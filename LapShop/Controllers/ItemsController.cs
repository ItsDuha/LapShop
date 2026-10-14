using LapShop.Bl;
using LapShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace LapShop.Controllers
{
    public class ItemsController : Controller
    {
        IItems oItem;
        IItemImages oItemImages;
        public ItemsController(IItems item, IItemImages itemImages)
        {
            oItem = item;
            oItemImages= itemImages;

        }
        
        public IActionResult ItemsDetails( int id)
        {
            var item = oItem.GetItemById(id);
            VmItemDetails vm = new VmItemDetails();
            vm.Item = item;
            vm.lstRecommendedItems = oItem.GetRecommendedItems(id).Take(20).ToList();
            vm.lstItemImages= oItemImages.GetByItemId(id);
            return View(vm);
        }

        public IActionResult ItemList()
        {
         
            return View();
        }
    }
}
