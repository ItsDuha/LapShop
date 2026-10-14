using LapShop.Models;

namespace LapShop.Bl
{

    public interface IItemTypes
    {
        public List<TbItemType> GetAll();
        public TbItemType GetById(int id);
        public bool Save(TbItemType itemType);
        public bool Delete(int id);

    }
    public class ClsItemTypes : IItemTypes
    {

        LapShopContext context;
        public ClsItemTypes(LapShopContext ctx)
        {
            this.context = ctx;
        }


        //List
        public List<TbItemType> GetAll()
        {
            try
            {
                
                var lstCategories = context.TbItemTypes.Where(a=> a.CurrentState==1).ToList();
                return lstCategories;

            }
            catch
            {
                return new List<TbItemType>();
            }


        }

        //Edit
        public TbItemType GetById(int id)
        {
            try
            {
               
                var item = context.TbItemTypes.FirstOrDefault(c => c.ItemTypeId == id && c.CurrentState==1);
                return item;
            }
            catch
            {
                return new TbItemType();
            }
        }

        public bool Save(TbItemType itemType)
        {
            try
            {
                
                if (itemType.ItemTypeId == 0)
                {


                    itemType.CreatedBy = "1";
                    itemType.CreatedDate = DateTime.Now;
                    context.TbItemTypes.Add(itemType);



                }
                else
                {
                    itemType.UpdatedBy = "1";
                    itemType.UpdatedDate = DateTime.Now;
                    context.Entry(itemType).State = Microsoft.EntityFrameworkCore.EntityState.Modified;


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
                
                var itemType = GetById(id);
                itemType.CurrentState = 0;
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
