using LapShop.Models;

namespace LapShop.Bl
{

    public interface IItemImages
    {

        public List<TbItemImage> GetByItemId(int id);

    }
    public class ClsItemImages : IItemImages
    {
        LapShopContext context;
        public ClsItemImages(LapShopContext ctx)
        {
            this.context = ctx;
        }


      
        public List<TbItemImage> GetByItemId(int id)
        {
            try
            {

                var item = context.TbItemImages.Where(c => c.ItemId ==id).ToList( );
                return item;
            }
            catch
            {
                return new List<TbItemImage>();
            }
        }


    }
}