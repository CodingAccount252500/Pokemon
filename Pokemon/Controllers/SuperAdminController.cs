using Microsoft.AspNetCore.Mvc;

namespace Pokemon.Controllers
{
    public class SuperAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
