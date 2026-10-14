using LapShop.Models;

namespace LapShop.Bl
{

    public interface IItems
    {
        public List<TbItem> GetAll();
        public List<VwItem> GetAllItemsData(int? categoryId);
        public List<VwItem> GetRecommendedItems(int itemId);
        public TbItem GetById(int id);
        public VwItem GetItemById(int id);
        public bool Save(TbItem item);
        public bool Delete(int id);
    }
    public class ClsItems : IItems
    {
        LapShopContext context;
        public ClsItems(LapShopContext ctx)
        {
            this.context = ctx;
        }
        //List
        public List<TbItem> GetAll()
        {
            try
            {
              
                var lstItems = context.TbItems.Where(i=> i.CurrentState==1).ToList();
                return lstItems;

            }
            catch
            {
                return new List<TbItem>();
            }


        }


        public List<VwItem> GetAllItemsData(int? categoryId)
        {
            try
            {
               
             
                var lstCategories = context.VwItems.Where(a => (a.CategoryId == categoryId || categoryId == null || categoryId == 0)
                && a.CurrentState==1 && !string.IsNullOrEmpty( a.ItemName)).OrderByDescending(c=> c.CreatedDate).ToList();
                
                return lstCategories;

            }
            catch
            {
                return new List<VwItem>();
            }


        }


        public List<VwItem> GetRecommendedItems(int itemId)
        {
            try
            {
                var item = GetById(itemId);
                var lstCategories = context.VwItems.Where(a => a.SalesPrice>item.SalesPrice-150 && a.SalesPrice<item.SalesPrice+150
                && a.CurrentState == 1).OrderByDescending(c => c.CreatedDate).ToList();

                return lstCategories;

            }
            catch
            {
                return new List<VwItem>();
            }


        }

        //Edit
        public TbItem GetById(int id)
        {
            try
            {
               
                var item = context.TbItems.FirstOrDefault(c => c.ItemId == id && c.CurrentState==1);
                return item;
            }
            catch
            {
                return new TbItem();
            }
        }

        public VwItem GetItemById(int id)
        {
            try
            {

                var item = context.VwItems.FirstOrDefault(c => c.ItemId == id && c.CurrentState == 1);
                return item;
            }
            catch
            {
                return new VwItem();
            }
        }

        public bool Save(TbItem item)
        {
            try
            {
               
                
                if (item.ItemId == 0)
                {

                    item.CurrentState = 1; // add edit
                    item.CreatedBy = "1";
                    item.CreatedDate = DateTime.Now;
                    context.TbItems.Add(item);



                }
                else
                {
                    item.UpdatedBy = "1";
                    item.UpdatedDate = DateTime.Now;
                    context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;


                }
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool Delete(int id)
        {
            try
            {
              
                var item = GetById(id);
                item.CurrentState = 0; // mark as deleted
                context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
