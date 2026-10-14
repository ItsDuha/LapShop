using LapShop.Bl;
using LapShop.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LapShop.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        IItems oItem;

        public ItemsController(IItems iItem)
        {
            oItem = iItem;
        }
        // GET: api/<ItemsController>
        /// <summary>
        /// 
        /// Get all items from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse Get()
        {
            ApiResponse oApiRepose = new ApiResponse();
            oApiRepose.Data = oItem.GetAllItemsData(null)
                          .ToList();
            oApiRepose.Errors = null;
            oApiRepose.StatusCode = "200";
            return oApiRepose;
        }

        // GET api/<ItemsController>/5
        /// <summary>
        /// Get item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ApiResponse Get(int id)
        {
            ApiResponse oApiRepose = new ApiResponse();
            oApiRepose.Data = oItem.GetById(id);
            oApiRepose.Errors = null;
            oApiRepose.StatusCode = "200";
            return oApiRepose;
            
        }

        [HttpGet("GetByCategoryId/{categoryId}")]
        
        public ApiResponse GetByCategoryId(int categoryId)
        {
            ApiResponse oApiRepose = new ApiResponse();
            oApiRepose.Data = oItem.GetAllItemsData(categoryId);
            oApiRepose.Errors = null;
            oApiRepose.StatusCode = "200";
            return oApiRepose;
           
        }


        // POST api/<ItemsController>
        [HttpPost]
        public ApiResponse Post([FromBody] TbItem item)
        {
            try
            {
                oItem.Save(item);
                ApiResponse oApiRepose = new ApiResponse();
                oApiRepose.Data = "Done";
                oApiRepose.Errors = null;
                oApiRepose.StatusCode = "200";
                return oApiRepose;
               
            }
            catch(Exception ex)
            {
                ApiResponse oApiRepose = new ApiResponse();
                oApiRepose.Data = null;
                oApiRepose.Errors = ex.Message;
                oApiRepose.StatusCode = "502";
                return oApiRepose;
            }
            
        }

        //for delete
        [HttpPost]
        [Route("Delete")]
        public void Delete([FromBody] int id)
        {
            oItem.Delete(id);
        }



       
    }
}
