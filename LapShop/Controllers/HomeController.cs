using Microsoft.AspNetCore.Mvc;
using LapShop.Models;
using LapShop.Bl;

namespace LapShop.Controllers
{
    public class HomeController : Controller
    {
        IItems oClsItems;
        ISliders oClsSliders;
        public HomeController(IItems items , ISliders oSliders)
        {
            oClsItems = items;
            oClsSliders = oSliders;
        }
        
        public IActionResult Index()
        {
            VmHomePage vm = new VmHomePage();
            vm.lstAllItems = oClsItems.GetAllItemsData(null)
      .Where(x => x.ImageName != "10-0db05539-d43b-47fa-8e1d-3fe2669ca437.jpg")
      .Take(24)
      .ToList();
            vm.lstRecommendedProducts = oClsItems.GetAllItemsData(null).Skip(60).Take(10).ToList();
            vm.lstNewItems = oClsItems.GetAllItemsData(null).Skip(30).Take(10).ToList();
            vm.lstFreeDelivery = oClsItems.GetAllItemsData(null).Skip(40).Take(3).ToList();
            vm.lstSliders = oClsSliders.GetAll();
            return View(vm);
        }
    }
}
