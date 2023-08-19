using Microsoft.AspNetCore.Mvc;
using Pokemon.Models;

namespace Pokemon.Controllers
{
    public class StoreAdminController : Controller
    {
        private readonly HarmAmmanContext HarmAmmanContext;
        public StoreAdminController(HarmAmmanContext harmamman)
        {
            this.HarmAmmanContext = harmamman;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get Order By ID Method 
        /// Used DTOOrderResponse Model
        /// Afaf
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IActionResult GetOrderById(int ID )
        {

            var response = HarmAmmanContext.Orders.Where(x => x.OrderId == ID).SingleOrDefault();
             DTOOrderResponse orderresponse = new DTOOrderResponse(); 
            if (response == null) {
                return BadRequest("order id was not found");

            }
            orderresponse.Note = response.Note;
            orderresponse.Orderdate = response.Orderdate;
            orderresponse.Totalprice = response.Totalprice;
            orderresponse.PaymentmethodId = response.PaymentmethodId;

            return Ok(orderresponse);



            
        }
    }
}
