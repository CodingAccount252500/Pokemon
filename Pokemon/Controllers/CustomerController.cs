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
                return View("NoProductsFound");
            }
            catch (Exception ex)
            {
                return View("Error: " + ex.Message);
            }
        }
        #endregion

        #region Get Product By Id
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

                if (product != null)
                {
                    return View(product);
                }
                return View("ProductNotFound");
            }
            catch (Exception ex)
            {
                return View("Error: " + ex.Message);
            }
        }
        #endregion

        #region Get All Category Products
        public async Task<IActionResult> GetAllCategoryProducts()
        {
            try
            {
                var categoryProducts = await _context.CategoryProducts.ToListAsync();

                if (categoryProducts != null && categoryProducts.Count > 0)
                {
                    return View(categoryProducts);
                }
                else
                {
                    return View("NoCategoriesFound");
                }
            }
            catch (Exception ex)
            {
                return View("Error: " + ex.Message);
            }
        }
        #endregion

        #region Filter Products
        public async Task<IActionResult> FilterProducts(int pageSize, int pageNumber, decimal? price, string name, string description)
        {
            try
            {
                IQueryable<Product> items = _context.Products;

                if (price != null)
                {
                    items = items.Where(x => x.Price >= price);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    items = items.Where(x => x.ProductName.Contains(name));
                }
                if (!string.IsNullOrEmpty(description))
                {
                    items = items.Where(x => x.Description.Contains(description));
                }

                int skipAmount = pageSize * (pageNumber - 1);
                List<Product> filteredProducts = await items.Skip(skipAmount).Take(pageSize).ToListAsync();

                return View(filteredProducts);
            }
            catch (Exception ex)
            {
                return View("Error: " + ex.Message);
            }
        }
        #endregion

        #region Add Product To Cart
        public IActionResult AddProductToCart(int productId, int qtn, int userId)
        {
            try
            {
                if (productId > 0 && qtn > 0 && userId > 0)
                {
                    var cart = _context.Carts.FirstOrDefault(x => x.UserId == userId && x.IsActive == true);
                    var product = _context.Products.FirstOrDefault(x => x.ProductId == productId);

                    if (cart != null && product != null)
                    {
                        var existingCartItem = _context.CartItems.FirstOrDefault(y => y.ProductId == productId && y.CartId == cart.CartId);

                        if (existingCartItem == null)
                        {
                            CartItem cartItem = new CartItem
                            {
                                ProductId = productId,
                                CartId = cart.CartId,
                                Quantity = qtn,
                                TotalPrice = (double)(product.Price * qtn)
                            };
                            _context.Add(cartItem);
                        }
                        else
                        {
                            existingCartItem.Quantity += qtn;
                            existingCartItem.TotalPrice = (double)(product.Price * existingCartItem.Quantity);
                            _context.Update(existingCartItem);
                        }

                        _context.SaveChanges();
                    }
                    else if (product != null)
                    {
                        Cart newCart = new Cart
                        {
                            UserId = userId,
                            IsActive = true
                        };
                        _context.Add(newCart);
                        _context.SaveChanges();

                        var cartNew = _context.Carts.FirstOrDefault(x => x.UserId == userId);
                        if (cartNew != null)
                        {
                            var existingCartItem = _context.CartItems.FirstOrDefault(y => y.ProductId == productId && y.CartId == cartNew.CartId);

                            if (existingCartItem == null)
                            {
                                CartItem cartItem = new CartItem
                                {
                                    ProductId = productId,
                                    CartId = cartNew.CartId,
                                    Quantity = qtn,
                                    TotalPrice = (double)(product.Price * qtn)
                                };
                                _context.Add(cartItem);
                            }
                            else
                            {
                                existingCartItem.Quantity += qtn;
                                existingCartItem.TotalPrice = (double)(product.Price * existingCartItem.Quantity);
                                _context.Update(existingCartItem);
                            }

                            _context.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error: " + ex.Message);
            }
        }
        #endregion
    }
}