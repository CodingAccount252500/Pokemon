using Microsoft.AspNetCore.Mvc;

namespace Pokemon.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
