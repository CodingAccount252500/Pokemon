using Microsoft.AspNetCore.Mvc;

namespace Pokemon.Controllers
{
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult SingleCategory()
        {
            return View();
        }
    }
}
