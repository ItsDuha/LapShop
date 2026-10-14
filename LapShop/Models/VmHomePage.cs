namespace LapShop.Models
{
    public class VmHomePage
    {
        public VmHomePage()
        {
            lstAllItems = new List<VwItem>();
            lstRecommendedProducts = new List<VwItem>();
            lstNewItems= new List<VwItem>();
            lstFreeDelivery = new List<VwItem>();
            lstCategories = new List<TbCategory>();
        }
        public List<VwItem> lstAllItems {  get; set; }
        public List<VwItem> lstRecommendedProducts {  get; set; }
        public List<VwItem>  lstNewItems{ get; set; }
        public List<VwItem>  lstFreeDelivery{ get; set; }
        public List<TbCategory>  lstCategories{ get; set; }

        public TbSetting Settings { get; set; }
        public List<TbSlider> lstSliders { get; set; }



    }
}
