namespace LapShop.Models
{
    public class ShoopingCart
    {
        public ShoopingCart()
        {
            lstItems = new List<ShoppingCartItem>();
        }
        public List<ShoppingCartItem> lstItems { get; set; }

        public decimal Total { get; set; }

        public string? PromoCode { get; set; }
    }
}
