using LapShop.Models;
using System.Runtime;
namespace LapShop.Bl
{
    public interface ISettings
    {
        public TbSetting GetAll();
        public bool Save(TbSetting setting);
    }

    public class ClsSettings : ISettings
    {
        LapShopContext context;
        public ClsSettings(LapShopContext ctx)
        {
            context = ctx;
        }
        public TbSetting GetAll()
        {
            try
            {
                var lstCategories = context.TbSetting.FirstOrDefault();
                return lstCategories;
            }
            catch
            {
                return new TbSetting();
            }
        }

        public bool Save(TbSetting setting)
        {
            try
            {
                context.Entry(setting).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
