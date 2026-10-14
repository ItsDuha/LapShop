using Domains;
using LapShop.Bl;
using LapShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LapShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        IItems itemService;
        UserManager<ApplicationUser> _userManager;
        ISalesInvoice salesInvoiceService;
        public OrderController(IItems itemservice, UserManager<ApplicationUser> userManager, ISalesInvoice ssalesInvoiceService)
        {
            itemService = itemservice;
            _userManager = userManager;
            salesInvoiceService = ssalesInvoiceService;
        }
        public IActionResult Cart()
        {
            ShoopingCart cart = new ShoopingCart();

            if (HttpContext.Request.Cookies["Cart"] != null)
            {
                string cookiesCart = HttpContext.Request.Cookies["Cart"];

                if (!string.IsNullOrEmpty(cookiesCart))
                {
                    cart = JsonConvert.DeserializeObject<ShoopingCart>(cookiesCart);

                    if (cart == null)
                    {
                        cart = new ShoopingCart();
                    }
                }
            }

            return View(cart);
        }

        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);

            var orders = salesInvoiceService.GetAll()
                            .Where(x => x.CustomerId == Guid.Parse(user.Id))
                            .OrderByDescending(x => x.InvoiceDate)
                            .ToList();

            return View(orders);
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            
            VmCheckout model = new VmCheckout();

           
            if (HttpContext.Request.Cookies["Cart"] != null)
            {
                model.Cart = JsonConvert.DeserializeObject<ShoopingCart>(
                    HttpContext.Request.Cookies["Cart"]);
            }

           
            if (model.Cart == null || !model.Cart.lstItems.Any())
            {
                return RedirectToAction("Cart");
            }

            
            var user = await _userManager.GetUserAsync(User);

           
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.Phone = user.PhoneNumber;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> OrderSuccess(VmCheckout model)
        {
            if (!ModelState.IsValid)
            {
                if (HttpContext.Request.Cookies["Cart"] != null)
                {
                    model.Cart = JsonConvert.DeserializeObject<ShoopingCart>(
                        HttpContext.Request.Cookies["Cart"]);
                }

                return View("Checkout", model);
            }

            if (HttpContext.Request.Cookies["Cart"] == null)
                return RedirectToAction("Cart");

            var cart = JsonConvert.DeserializeObject<ShoopingCart>(
                HttpContext.Request.Cookies["Cart"]);

            if (cart == null || !cart.lstItems.Any())
                return RedirectToAction("Cart");

            foreach (var item in cart.lstItems)
            {
                if (item.Qty < 1)
                    item.Qty = 1;
            }

            await SaveOrder(cart);

            //Send cart to success page
            model.Cart = cart;

            //Clear cart
            Response.Cookies.Delete("Cart");

            return View(model);
        }

        [Authorize]
        public IActionResult OrderDetails(int id)
        {
            var order = salesInvoiceService.GetById(id);

            return View(order);
        }

        public IActionResult AddToCart(int itemId, int qty = 1)
        {
            ShoopingCart cart;
            
            if (HttpContext.Request.Cookies["Cart"] != null)

                cart = JsonConvert.DeserializeObject<ShoopingCart>(HttpContext.Request.Cookies["Cart"]);


            else
                cart = new ShoopingCart();
            var item = itemService.GetById(itemId);

            var itemInList = cart.lstItems.Where(a => a.ItemId == itemId).FirstOrDefault();

            if (itemInList != null)
            {
                itemInList.Qty += qty;
                itemInList.Total = itemInList.Qty * itemInList.Price;
            }
            else
            {
                cart.lstItems.Add(new ShoppingCartItem
                {

                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Price = item.SalesPrice,
                    Qty = qty,
                    Total = item.SalesPrice * qty

                });
            }
               
            cart.Total= cart.lstItems.Sum(a => a.Total);
            HttpContext.Response.Cookies.Append("Cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Cart");


        }
        public IActionResult RemoveFromCart(int itemId)
        {
            ShoopingCart cart = new ShoopingCart();

            if (HttpContext.Request.Cookies["Cart"] != null)
            {
                string cookieCart = HttpContext.Request.Cookies["Cart"];

                if (!string.IsNullOrEmpty(cookieCart))
                {
                    cart = JsonConvert.DeserializeObject<ShoopingCart>(cookieCart);

                    if (cart == null)
                        cart = new ShoopingCart();
                }
            }

            var item = cart.lstItems.FirstOrDefault(x => x.ItemId == itemId);

            if (item != null)
            {
                cart.lstItems.Remove(item);
            }

            cart.Total = cart.lstItems.Sum(x => x.Total);

            HttpContext.Response.Cookies.Append(
                "Cart",
                JsonConvert.SerializeObject(cart)
            );

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult UpdateQty(int itemId, int qty)
        {
            ShoopingCart cart = new ShoopingCart();

            if (HttpContext.Request.Cookies["Cart"] != null)
            {
                cart = JsonConvert.DeserializeObject<ShoopingCart>(
                    HttpContext.Request.Cookies["Cart"]);
            }

            var item = cart.lstItems.FirstOrDefault(x => x.ItemId == itemId);

            if (item != null)
            {
                item.Qty = qty;
                item.Total = item.Price * qty;
            }

            cart.Total = cart.lstItems.Sum(x => x.Total);

            HttpContext.Response.Cookies.Append(
                "Cart",
                JsonConvert.SerializeObject(cart));

            return Ok();
        }



        async Task SaveOrder(ShoopingCart oShopingCart)
        {
            try
            {
                List<TbSalesInvoiceItem> lstInvoiceItems = new List<TbSalesInvoiceItem>();
                foreach (var item in oShopingCart.lstItems)
                {
                    lstInvoiceItems.Add(new TbSalesInvoiceItem()
                    {
                        ItemId = item.ItemId,
                        Qty = item.Qty,
                        InvoicePrice = item.Price
                    });
                }

                var user = await _userManager.GetUserAsync(User);

                TbSalesInvoice oSalesInvoice = new TbSalesInvoice()
                {
                    InvoiceDate = DateTime.Now,
                    CustomerId = Guid.Parse(user.Id),
                    DelivryDate = DateTime.Now.AddDays(5),
                    CreatedBy = user.Id,
                    CreatedDate = DateTime.Now
                };

                salesInvoiceService.Save(oSalesInvoice, lstInvoiceItems, true);
            }
            catch (Exception ex)
            {

            }
        }
    }


}
