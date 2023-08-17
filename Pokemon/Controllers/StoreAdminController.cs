using Microsoft.AspNetCore.Mvc;

namespace Pokemon.Controllers
{
    public class StoreAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
