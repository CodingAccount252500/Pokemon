using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokemon.DTO.Customer;
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
                return View("Error");
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
                return View("Error");
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
                return View("Error");
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
                return View("Error");
            }
        }
        #endregion

        #region Add Product To Cart
        public async Task<IActionResult> AddProductToCart(int productId, int qtn, int userId)
        {
            try
            {
                if (productId > 0 && qtn > 0 && userId > 0)
                {
                    var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive == true);
                    var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);

                    if (cart != null && product != null)
                    {
                        var existingCartItem = await _context.CartItems.FirstOrDefaultAsync(y => y.ProductId == productId && y.CartId == cart.CartId);

                        if (existingCartItem == null)
                        {
                            CartItem cartItem = new CartItem
                            {
                                ProductId = productId,
                                CartId = cart.CartId,
                                Quantity = qtn,
                                TotalPrice = (double)(product.Price * qtn)
                            };
                            await _context.AddAsync(cartItem);
                        }
                        else
                        {
                            existingCartItem.Quantity += qtn;
                            existingCartItem.TotalPrice = (double)(product.Price * existingCartItem.Quantity);
                            _context.Update(existingCartItem);
                        }

                        await _context.SaveChangesAsync();
                    }
                    else if (product != null)
                    {
                        Cart newCart = new Cart
                        {
                            UserId = userId,
                            IsActive = true
                        };
                        await _context.AddAsync(newCart);
                        await _context.SaveChangesAsync();

                        var cartNew = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
                        if (cartNew != null)
                        {
                            var existingCartItem = await _context.CartItems.FirstOrDefaultAsync(y => y.ProductId == productId && y.CartId == cartNew.CartId);

                            if (existingCartItem == null)
                            {
                                CartItem cartItem = new CartItem
                                {
                                    ProductId = productId,
                                    CartId = cartNew.CartId,
                                    Quantity = qtn,
                                    TotalPrice = (double)(product.Price * qtn)
                                };
                                await _context.AddAsync(cartItem);
                            }
                            else
                            {
                                existingCartItem.Quantity += qtn;
                                existingCartItem.TotalPrice = (double)(product.Price * existingCartItem.Quantity);
                                _context.Update(existingCartItem);
                            }

                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion

        #region Get Order Details
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);

                if (order != null)
                {
                    var orderDetails = new OrdedDetailsForUserDTOres
                    {
                        OrderDate = order.Orderdate.ToString(),
                        DeliveryDate = order.Deliverydate.ToString(),
                        TotalPrice = order.Totalprice.ToString(),
                        Note = order.Note,
                        OrderStatus = _context.OrderStates.FirstOrDefault(x => x.OrderStateId == order.OrderStateId)?.Name,
                    };

                    var cart = await _context.Carts.FirstOrDefaultAsync(x => x.CartId == order.CartId);
                    var cartItems = _context.CartItems
                        .Where(x => x.CartId == order.CartId)
                        .Join(_context.Products, cit => cit.ProductId, it => it.ProductId, (cit, it) => new OrderCatrItemDTOreq
                        {
                            Id = it.ProductId.ToString(),
                            Name = it.ProductName,
                            Price = it.Price.ToString(),
                            Quantity = cit.Quantity.ToString(),
                            NetPrice = cit.TotalPrice.ToString()
                        })
                        .ToList();

                    orderDetails.MyCart = cartItems;
                    return View(orderDetails);
                }
                else
                {
                    return View("OrderNotFound");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion

        #region Check Order Status
        public async Task<IActionResult> CheckOrderStatus(int id)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == id);

                if (order != null)
                {
                    var orderStateName = _context.OrderStates.FirstOrDefault(x => x.OrderStateId == order.OrderStateId)?.Name;
                    return View(orderStateName);
                }
                else
                {
                    return View("OrderNotFound");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion

        #region Create New Order
        public async Task<IActionResult> CreateNewOrder(OrderDTOreq order)
        {
            try
            {
                var cart = await _context.Carts.FirstOrDefaultAsync(x => x.CartId == order.CartId && x.IsActive == true);

                if (cart != null && order.DeliveryDate.AddDays(-2).AddMinutes(1) > DateTime.Now)
                {
                    cart.IsActive = false;
                    _context.Update(cart);
                    await _context.SaveChangesAsync();

                    Order newOrder = new Order
                    {
                        CartId = order.CartId,
                        Deliverydate = order.DeliveryDate,
                        Orderdate = DateTime.Now,
                        Note = order.Note,
                        OrderStateId = 1,
                        Totalprice = await _context.CartItems
                            .Where(x => x.CartId == order.CartId)
                            .SumAsync(x => x.TotalPrice)
                    };

                    await _context.AddAsync(newOrder);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return View("InvalidOrder");
                }

                return RedirectToAction("OrderConfirmation"); 
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        #endregion

    }
}