
using Lexiflix.Models.Db;
using Lexiflix.Models.ViewModels;
using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Lexiflix.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly ICustomerServices _customerServices;
        private readonly IMovieServices _movieServices;


        private const string CartSessionKey = "Cart";

        public OrderController(IOrderServices orderServices, ICustomerServices customerServices, IMovieServices movieServices)
        {
            _orderServices = orderServices;
            _customerServices = customerServices;
            _movieServices = movieServices;
        }




        [HttpGet]

        public IActionResult Create()
        {
            var viewModel = new OrderVM
            {

                OrderRows = new List<OrderRowVM>
                    {
                        new OrderRowVM()
                    }

              PopulateDropdowns();


            
                return View(viewModel);
        }



        [HttpPost]



    public IActionResult Create(OrderVM orderVM)
    {
        if (orderVM.CustomerId == null || !_customerServices.Exists(orderVM.CustomerId.Value))
        {
            //if (orderVM.CustomerId == null || !_customerServices.Exists(orderVM.CustomerId.Value))
            //{
            //    ModelState.AddModelError("CustomerId", "Please select a valid customer.");
            //}

            if (ModelState.IsValid)
            {
                _orderServices.AddOrderByAdmin(orderVM);
                return RedirectToAction("Index");
            }



         PopulateDropdowns();

            

        return View(orderVM);

    }
    private void PopulateDropdowns()
    {
        var movies = _movieServices.GetAllMovies() ?? new List<Movie>();
        var customers = _customerServices.GetAllCustomers() ?? new List<Customer>();

        ViewBag.MovieList = new SelectList(movies, "Id", "Title");
        ViewBag.CustomerList = new SelectList(customers.Select(c => new { c.Id, FullName = c.FirstName + " " + c.LastName }), "Id", "FullName");
    }








           [HttpGet]

        public IActionResult Detail(int id)
        {
            var order = _orderServices.GetOrderWithDetails(id);
            if (order == null)
                return NotFound();

            return View(order);
        }



        public IActionResult Index(string searchString, int? pageNumber, int pageSize = 10)

        {
            var orders = _orderServices.GetAllOrders(searchString, pageNumber ?? 1, pageSize);
            return View(orders);
        }

        public IActionResult OrderDetail()
        {
            //var orders = _orderServices.GetAllOrders();
            //if (orders == null)
            //    return NotFound();
            return View();
        }


        public IActionResult ViewCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);

            if (cart == null)
            {
                cart = new List<OrderRow>();
            }

            // Load movie details for each cart item
            foreach (var item in cart)
            {
                if (item.MovieId > 0)
                {
                    item.Movie = _orderServices.GetMovieById(item.MovieId);
                }
            }

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
            var existingItem = cart.FirstOrDefault(x => x.MovieId == request.MovieId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new OrderRow
                {
                    MovieId = request.MovieId,
                    Price = request.Price,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

            // Calculate totals
            var totalItems = cart.Sum(item => item.Quantity);
            var totalPrice = cart.Sum(item => item.Price * item.Quantity);
            var itemQuantity = cart.FirstOrDefault(x => x.MovieId == request.MovieId)?.Quantity ?? 0;

            return Json(new
            {
                success = true,
                totalItems,
                totalPrice,
                itemQuantity
            });
        }



        [HttpPost]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
            var existingItem = cart.FirstOrDefault(x => x.MovieId == request.MovieId);

            bool itemRemoved = false;

            if (existingItem != null)
            {
                if (request.Action == "increase")
                {
                    existingItem.Quantity++;
                }
                else if (request.Action == "decrease")
                {
                    if (existingItem.Quantity > 1)
                    {
                        existingItem.Quantity--;
                    }
                    else
                    {
                        cart.Remove(existingItem);
                        itemRemoved = true;
                    }
                }
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

            // Calculate totals
            var totalItems = cart.Sum(item => item.Quantity);
            var totalPrice = cart.Sum(item => item.Price * item.Quantity);
            var itemQuantity = itemRemoved ? 0 : cart.FirstOrDefault(x => x.MovieId == request.MovieId)?.Quantity ?? 0;

            return Json(new
            {
                success = true,
                movieId = request.MovieId,
                totalItems,
                totalPrice,
                itemQuantity,
                itemRemoved
            });
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
            var existingItem = cart.FirstOrDefault(x => x.MovieId == request.MovieId);

            if (existingItem != null)
            {
                if (request.RemoveAll)
                {
                    cart.Remove(existingItem);
                }
                else if (existingItem.Quantity > 1)
                {
                    existingItem.Quantity--;
                }
                else
                {
                    cart.Remove(existingItem);
                }
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

            // Calculate totals
            var totalItems = cart.Sum(item => item.Quantity);
            var totalPrice = cart.Sum(item => item.Price * item.Quantity);

            return Json(new
            {
                success = true,
                movieId = request.MovieId,
                totalItems,
                totalPrice,
                itemQuantity = 0 // Since we're removing the item
            });
        }

        public class RemoveFromCartRequest
        {
            public int MovieId { get; set; }
            public bool RemoveAll { get; set; }
        }

        [HttpPost]
        public IActionResult Checkout(string email, Customer customer, bool sameAsBilling = false)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
            if (cart == null || cart.Count == 0)
                return RedirectToAction("ViewCart");

            // If same as billing, copy billing address to delivery address
            if (sameAsBilling)
            {
                customer.DeliveryAddress = customer.BillingAddress;
                customer.DeliveryCity = customer.BillingCity;
                customer.DeliveryZip = customer.BillingZip;
            }

            var existingCustomer = _customerServices.GetCustomerByEmail(email);
            if (existingCustomer == null)
            {
                // New customer - add to database
                customer.Email = email;
                _customerServices.AddNewCustomer(customer);
                existingCustomer = customer;
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerId = existingCustomer.Id,
                OrderRows = cart
            };

            _orderServices.CreateOrder(order);
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("OrderSuccess", new { orderId = order.Id });
        }

        [HttpGet]
        public IActionResult GetCartSummary()
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();

            var totalItems = cart.Sum(item => item.Quantity);
            var totalPrice = cart.Sum(item => item.Price * item.Quantity);

            return Json(new { totalItems, totalPrice });
        }

        [HttpGet]
        public IActionResult GetItemQuantity(int movieId)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
            var item = cart.FirstOrDefault(x => x.MovieId == movieId);

            return Json(new { quantity = item?.Quantity ?? 0 });
        }

        public IActionResult OrderSuccess(int orderId)
        {
            var order = _orderServices.GetOrderWithDetails(orderId);

            return View(order);
        }

        public class AddToCartRequest
        {
            public int MovieId { get; set; }
            public decimal Price { get; set; }
        }

        public class UpdateQuantityRequest
        {
            public int MovieId { get; set; }
            public string Action { get; set; }
        }

        //Get: Delet confirmation page
        public IActionResult Delete(int Id)

        {
            return View();  /*shows Delete.cshtml*/
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)

        {
            _orderServices.DeleteOrder(id); /*pass the id*/
            TempData["SuccessMessage"] = "The order has been deleted successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _orderServices.GetOrderWithDetails(id);
            if (order == null)
                return NotFound();

            var orderVM = new OrderVM
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                CustomerEmail = order.Customer.Email,
                CustomerName = $"{order.Customer.FirstName} {order.Customer.LastName}",
                DeliveryAddress = $"{order.Customer.DeliveryAddress}, {order.Customer.DeliveryCity}, {order.Customer.DeliveryZip}",
                OrderRows = order.OrderRows.Select(or => new OrderRowVM
                {
                    Id = or.Id,
                    MovieId = or.MovieId,
                    MovieTitle = or.Movie?.Title,
                    Quantity = or.Quantity,
                    Price = or.Price
                }).ToList()
            };

            // Ensure at least one empty row for adding new movies
            if (!orderVM.OrderRows.Any())
            {
                orderVM.OrderRows.Add(new OrderRowVM());
            }

            // Populate movies dropdown
            ViewBag.Movies = _movieServices.GetAllMovies().Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = $"{m.Title} - Kr{m.Price}"
            }).ToList();

            return View(orderVM);
        }

        [HttpPost]
        public IActionResult Edit(OrderVM orderVM)
        {
            if (orderVM.Id == null)
                return BadRequest();

            // Validate that at least one order row exists with valid data
            var validRows = orderVM.OrderRows.Where(row =>
                row.MovieId.HasValue &&
                row.Quantity.HasValue &&
                row.Quantity > 0 &&
                row.Price.HasValue &&
                row.Price > 0).ToList();

            if (!validRows.Any())
            {
                ModelState.AddModelError("", "At least one movie must be selected with valid quantity and price.");
            }

            if (ModelState.IsValid)
            {
                _orderServices.UpdateOrder(orderVM);
                TempData["SuccessMessage"] = "Order updated successfully.";
                //return RedirectToAction("Detail", new { id = orderVM.Id });
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            // Reload customer info for display
            var existingOrder = _orderServices.GetOrderWithDetails(orderVM.Id.Value);
            if (existingOrder != null)
            {
                orderVM.CustomerEmail = existingOrder.Customer.Email;
                orderVM.CustomerName = $"{existingOrder.Customer.FirstName} {existingOrder.Customer.LastName}";
                orderVM.DeliveryAddress = $"{existingOrder.Customer.DeliveryAddress}, {existingOrder.Customer.DeliveryCity}, {existingOrder.Customer.DeliveryZip}";
            }

            // Repopulate movies dropdown
            ViewBag.Movies = _movieServices.GetAllMovies().Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = $"{m.Title} - Kr{m.Price}"
            }).ToList();

            return View(orderVM);
        }
    }
}