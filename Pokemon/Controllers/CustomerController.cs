using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokemon.Models;

namespace Pokemon.Controllers
{
    public class CustomerController : Controller
    {
        #region Field & Property
        private readonly HarmAmmanContext _context;
        #endregion

        #region Constructor
        public CustomerController(HarmAmmanContext _context) => _context = _context;
        #endregion

        #region Index URL
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Get All Products
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _context.Products.ToListAsync();

                if (products != null && products.Count > 0)
                {
                    return View(products);
                }
                else
                {
                    return View("NoProductsFound");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion
    }
}
