using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Pokemon.DTO;
using Pokemon.DTO.Customer;
using Pokemon.Models;
using System.Linq;
using DTOOrderResponse = Pokemon.DTO.DTOOrderResponse;

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

            try
            {

            var response = HarmAmmanContext.Orders.Where(x => x.OrderId == ID).SingleOrDefault();
             DTOOrderResponse orderresponse = new DTOOrderResponse(); 
            if (response == null) {
                return BadRequest("order id was not found");

            }
            orderresponse.Note = response.Note;
                orderresponse.Orderdate = response.Orderdate.ToString();
                orderresponse.Totalprice = response.Totalprice.ToString();
                orderresponse.Deliverydate = response.Deliverydate.ToString();
                orderresponse.Paymentmethod = HarmAmmanContext.PaymentMethods.Where(x => x.PaymentmethodId == response.PaymentmethodId).First().Name;
                orderresponse.OrderState = HarmAmmanContext.OrderStates.Where(x => x.OrderStateId == response.OrderStateId).First().Name.ToString();


                var cart = HarmAmmanContext.Carts.Where(x => x.CartId == response.CartId).ToList();
                var cartitem = HarmAmmanContext.CartItems.Where(x => x.CartId == response.CartId).ToList();
                var item = HarmAmmanContext.Products.ToList();
                var categoryproduct = HarmAmmanContext.CategoryProducts.ToList();
                var orderItems = from c in cart
                                 join cit in cartitem
                                 on c.CartId equals cit.CartId
                                 join it in item
                                 on cit.ProductId equals it.ProductId
                                 join cid in categoryproduct
                                 on it.CategoryProductId equals cid.CategoryProductId
                                 select new DTOProductDetails
                                 {

                                     Name = it.ProductName,
                                     Price = it.Price.ToString(),
                                     Description = it.Description,
                                     Quantity = cit.Quantity.ToString(),
                                     category = HarmAmmanContext.CategoryProducts.Where(x => x.CategoryProductId == cid.CategoryProductId).First().Name


                                 };
                orderresponse.MyCart = orderItems.ToList();
            return Ok(orderresponse);


            }
            catch(  Exception ex)
            { return BadRequest(ex.Message ); }



        }

        /// <summary>
        /// Add Media to Product 
        /// Step 1 check if Media type already exist >> if not add it 
        /// Step 2 In all cases Add the Media to the selected product 
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        public IActionResult AddMediatoProduct(DTOMediaRequest media)
        {
            //DTOMediaRequest media
            try
            {

                bool MediaType_ = HarmAmmanContext.MediaTypes.Where(x => x.Name == media.MediaType).Any();

                if (!MediaType_)
                {
                    Models.MediaType md = new Models.MediaType();
                    md.Name = media.MediaType;
                    HarmAmmanContext.MediaTypes.Add(md);
                    HarmAmmanContext.SaveChanges();


                }
                var SelectedMediaType = HarmAmmanContext.MediaTypes.Where(x => x.Name == media.MediaType).FirstOrDefault();

                Models.Medium media_ = new Models.Medium();
                media_.IsMainMedia = media.IsMainMedia;

                media_.MediaPath = media.MediaPath;

                media_.MediatypeId = SelectedMediaType.MediatypeId;
                media_.ProductId = media.ProductId;


                HarmAmmanContext.Media.Add(media_);
                HarmAmmanContext.SaveChanges();



                return Ok("Media Added Successfully");
            }
            catch(Exception ex)
            {

                return BadRequest(ex.Message ); 
            }
        }

        //////
        ///Delete Product Request
        ///Step 1 . Check if Media exist 
        ///Step 2 . Delete it 
        ///
        public IActionResult DeleteProductMedia(int ID)
        {

            try
            {
            
                var media = HarmAmmanContext.Media.Where(x => x.MediaId == ID).SingleOrDefault();
                if (media == null)
                    return NotFound();
                HarmAmmanContext.Remove(media);
                HarmAmmanContext.SaveChanges();


                return Ok("Media Deleted Successfully");

            }
            catch(Exception ex) {
                
                return BadRequest(ex.Message);
            
            }
        }


        public IActionResult ProcessOrder (int ID)
        {
            try
            {

                var Order_ = HarmAmmanContext.Orders.Where(x => x.OrderId == ID).SingleOrDefault();
                if (Order_ == null) return NotFound();
                OrderState orderstate = HarmAmmanContext.OrderStates.Where(x => x.Name == "Processed").SingleOrDefault();
                if (orderstate == null) return NotFound();

                Order_.OrderStateId = orderstate.OrderStateId;
                HarmAmmanContext.Update(Order_);
                HarmAmmanContext.SaveChanges();
                return Ok("Order " + ID + " status changed to Processed");
            }
            catch(Exception ex) { return BadRequest(ex.Message); }
        }
        
        /// <summary>
        /// Add New Action (AddCategory)
        /// Solved by Bahaa
        /// We used CategoryDTO class as object of parameter
        /// </summary>
        /// <param name="categotydto"></param>
        /// <returns></returns>
        
        [HttpPost]
        public IActionResult AddCategory(CategoryDTO categotydto)
        {
            if ( HarmAmmanContext.Categories.Any(x => x.Name == categotydto.catname))
            {
                return BadRequest("The CategoryName is Already Excist");
            }

            Category category = new Category();
            category.Name = categotydto.catname;
            HarmAmmanContext.Add(category);
            HarmAmmanContext.SaveChanges();
            return Ok("Add Category Successfully");
            
        }

        /// <summary>
        /// GetAllCategory
        /// </summary>
        /// <returns></returns>
        public IActionResult GetAllCategoreies(int PageSize, int PageNumber)
        {
            try
            {
                var getcategory = HarmAmmanContext.Categories.ToList();

                int skipAmount = PageSize * PageNumber - (PageSize);
                return Ok(getcategory.Skip(skipAmount).Take(PageSize));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /// <summary>
        /// Commit By Marwa
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 

        public IActionResult GetCategoryById(int Id)
        {
            var category = HarmAmmanContext.Categories.Where(x => x.CategoryId == Id).SingleOrDefault();
            CategoryDTO categorydto = new CategoryDTO();

            categorydto.catname = category.Name;
            return Ok(categorydto);
        }

        /// <summary>
        /// UpdateCategory done by marwa
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public IActionResult UpdateCategory(int Id, CategoryDTO update)
        {

            var checkcategory = HarmAmmanContext.Categories.Where(x => x.CategoryId == Id).SingleOrDefault();
            if (checkcategory != null)
            {
                if (checkcategory.Name != null)
                {
                    checkcategory.Name = update.catname;

                    HarmAmmanContext.Update(checkcategory);
                    HarmAmmanContext.SaveChanges();
                    return Ok("Category Has Been Updated");
                }
            }
            return Ok("Category Not Available");
        }
        /// <summary>
        /// DeleteCategory By Marwa

        /// <param name="Id"></param>
        /// <returns></returns>

        public IActionResult DeleteCategory(int Id)
        {
            var category = HarmAmmanContext.Categories.Where(x => x.CategoryId == Id).SingleOrDefault();
            if (category != null)
            {

                HarmAmmanContext.Remove(category);
                HarmAmmanContext.SaveChanges();




                return Ok("Category Deleted Successfully");
            }
            return Ok("Category Not Available");
        }

        /// <summary>
        /// GetAllProducts Method
        /// Solved by Bahaa
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>

        public IActionResult GetAllProducts(int pageSize, int pageNumber)
        {
            try
            {
                var products = HarmAmmanContext.Products.ToList();
                int pageSkipe = pageSize * pageNumber - pageSize;
                return Ok(products.Skip(pageSkipe).Take(pageSize));

            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// GetAllOrders
        /// Solved by Bahaa
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <returns></returns>
        public IActionResult GetAllOrders(int PageSize, int PageNumber)
        {

            try
            {
                DTOOrderResponse response = new DTOOrderResponse();
                var orders = HarmAmmanContext.Orders.Where(x => x.Orderdate.ToString() == response.Orderdate).FirstOrDefault();
                response.Orderdate = orders.Orderdate.ToString();
                response.Deliverydate = orders.Deliverydate.ToString();
                response.Totalprice = orders.Totalprice.ToString();
                response.Note = orders.Note;
                response.Paymentmethod = HarmAmmanContext.PaymentMethods.Where(x => x.PaymentmethodId == orders.PaymentmethodId).FirstOrDefault().Name;
                response.Orderdate = HarmAmmanContext.OrderStates.Where(x => x.OrderStateId == orders.OrderStateId).FirstOrDefault().Name;


                var carts = HarmAmmanContext.Carts.Where(x => x.CartId == orders.CartId).ToList();
                var cartitems = HarmAmmanContext.CartItems.Where(x => x.CartId == orders.CartId).ToList();
                var products = HarmAmmanContext.Products.ToList();
                var catproducts = HarmAmmanContext.CategoryProducts.ToList();

                var jorders = from c in carts
                              join cpr in cartitems
                              on c.CartId equals cpr.CartId
                              join pr in products
                              on cpr.ProductId equals pr.ProductId
                              join catp in catproducts
                              on pr.CategoryProductId equals catp.CategoryProductId
                              select new DTOProductDetails
                              {
                                  Name = pr.ProductName,
                                  Price = pr.Price.ToString(),
                                  Quantity = cpr.Quantity.ToString(),
                                  Description = pr.Description,
                                  category = HarmAmmanContext.Categories.Where(x => x.CategoryId == catp.CategoryProductId).FirstOrDefault().Name

                              };

                response.MyCart = jorders.ToList();
                int PageSkip = PageNumber * PageSize - PageSize;
                return Ok(products.Skip(PageSkip).Take(PageSize));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

    }
}
