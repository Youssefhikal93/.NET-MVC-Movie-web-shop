//using Lexiflix.Models;
//using Lexiflix.Services;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;

//namespace Lexiflix.Controllers
//{
//    public class OrderController : Controller
//    {
//        private readonly IOrderServices _orderServices;
//        private readonly ICustomerServices _customerServices;

//        private const string CartSessionKey = "Cart";

//        public OrderController(IOrderServices orderServices, ICustomerServices customerServices)
//        {
//            _orderServices = orderServices;
//            _customerServices = customerServices;

//        }
//        public IActionResult Index()
//        {
//            var orders = _orderServices.GetAllOrders();
//            return View(orders);
//        }
//        public IActionResult OrderDetail()
//        {
//            var orders = _orderServices.GetAllOrders();
//            if (orders == null)
//                return NotFound();
//            return View(orders);
//        }
//        public IActionResult Create()
//        {
//            return View();
//        }

//        public IActionResult ViewCart()
//        {
//            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
//            foreach (var row in cart)
//            {

//            }
//            return View(cart);

//        }

//        //[HttpPost] old sayda
//        //public IActionResult AddToCart(int movieId, decimal price)
//        //{
//        //    Console.WriteLine($"DEBUG: Added movieId {movieId} with price {price} to cart");
//        //    var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//        //    var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson)?? new List<OrderRow>();
//        //    var existingItem = cart.FirstOrDefault(x => x.MovieId == movieId);


//        //    if (existingItem != null)
//        //    {
//        //        cart.FirstOrDefault(x => x.MovieId == movieId).Quantity++;
//        //    }
//        //    else
//        //    {
//        //        cart.Add(new OrderRow { MovieId = movieId, Price = price, Quantity = 1 });

//        //    }
//        //    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
//        //    return RedirectToAction("ViewCart");


//        //}
//        [HttpPost]
//        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
//        {
//            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
//            var existingItem = cart.FirstOrDefault(x => x.MovieId == request.MovieId);

//            if (existingItem != null)
//            {
//                existingItem.Quantity++;
//            }
//            else
//            {
//                cart.Add(new OrderRow
//                {
//                    MovieId = request.MovieId,
//                    Price = request.Price,
//                    Quantity = 1
//                });
//            }

//            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

//            // Calculate totals
//            var totalItems = cart.Sum(item => item.Quantity);
//            var totalPrice = cart.Sum(item => item.Price * item.Quantity);

//            return Json(new
//            {
//                success = true,
//                totalItems,
//                totalPrice
//            });
//        }


//        //[HttpPost]
//        //public IActionResult RemoveFromCart(int movieId)
//        //{
//        //    var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//        //    var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
//        //    var existingItem = cart.FirstOrDefault(x => x.MovieId == movieId);

//        //    if (existingItem != null)
//        //    {
//        //        if (existingItem.Quantity > 1)
//        //        {
//        //            existingItem.Quantity--;
//        //        }
//        //        else
//        //        {
//        //            cart.Remove(existingItem);


//        //        }
//        //        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
//        //    }
//        //    return RedirectToAction("ViewCart");
//        //}

//        [HttpPost]
//        public IActionResult Checkout(string email, Customer customer)
//        {
//            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
//            if (cart.Count == 0)
//                return RedirectToAction("ViewCart");

//            var existingCustomer = _customerServices.GetCustomerByEmail(email);
//            if (existingCustomer == null)
//            {
//                _customerServices.AddNewCustomer(customer);
//                existingCustomer = customer;
//            }
//            var order = new Order
//            {
//                OrderDate = DateTime.Now,
//                CustomerId = existingCustomer.Id,
//                OrderRows = cart
//            };
//            _orderServices.CreateOrder(order);
//            HttpContext.Session.Remove("Cart");
//            return RedirectToAction("OrderSuccess");

//        }
//        [HttpGet]
//        public IActionResult GetCartSummary()
//        {
//            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();

//            var totalItems = cart.Sum(item => item.Quantity);
//            var totalPrice = cart.Sum(item => item.Price * item.Quantity);

//            return Json(new { totalItems, totalPrice });
//        }

//        //[HttpPost]

//        //public IActionResult AddCopy(int movieId)
//        //{

//        //    var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//        //    var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);

//        //    var existingItem = cart.FirstOrDefault(x => x.MovieId == movieId);
//        //    if (existingItem != null)
//        //    {
//        //        existingItem.Quantity++; // increase copies
//        //    }

//        //    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

//        //    return RedirectToAction("ViewCart");
//        //}


//        public IActionResult OrderSuccess()
//        {
//            return View();
//        }

//        //[HttpPost]
//        //public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
//        //{
//        //    var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//        //    var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
//        //    var existingItem = cart.FirstOrDefault(x => x.MovieId == request.MovieId);

//        //    if (existingItem != null)
//        //    {
//        //        existingItem.Quantity++;
//        //    }
//        //    else
//        //    {
//        //        cart.Add(new OrderRow
//        //        {
//        //            MovieId = request.MovieId,
//        //            Price = request.Price,
//        //            Quantity = 1
//        //        });
//        //    }

//        //    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

//        //    // Calculate totals
//        //    var totalItems = cart.Sum(item => item.Quantity);
//        //    var totalPrice = cart.Sum(item => item.Price * item.Quantity);
//        //    var itemQuantity = cart.FirstOrDefault(x => x.MovieId == request.MovieId)?.Quantity ?? 0;

//        //    return Json(new
//        //    {
//        //        success = true,
//        //        totalItems,
//        //        totalPrice,
//        //        itemQuantity
//        //    });
//        //}

//        //[HttpPost]
//        //public async Task<IActionResult> AddCopy([FromBody] AddToCartRequest request)
//        //{
//        //    var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//        //    var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
//        //    var existingItem = cart.FirstOrDefault(x => x.MovieId == request.MovieId);

//        //    if (existingItem != null)
//        //    {
//        //        existingItem.Quantity++;
//        //    }

//        //    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

//        //    // Calculate totals
//        //    var totalItems = cart.Sum(item => item.Quantity);
//        //    var totalPrice = cart.Sum(item => item.Price * item.Quantity);
//        //    var itemQuantity = existingItem?.Quantity ?? 0;

//        //    return Json(new
//        //    {
//        //        success = true,
//        //        totalItems,
//        //        totalPrice,
//        //        itemQuantity
//        //    });
//        //}

//        //[HttpPost]
//        //public async Task<IActionResult> RemoveFromCart([FromBody] AddToCartRequest request)
//        //{
//        //    var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
//        //    var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
//        //    var existingItem = cart.FirstOrDefault(x => x.MovieId == request.MovieId);

//        //    if (existingItem != null)
//        //    {
//        //        if (existingItem.Quantity > 1)
//        //        {
//        //            existingItem.Quantity--;
//        //        }
//        //        else
//        //        {
//        //            cart.Remove(existingItem);
//        //        }

//        //        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
//        //    }

//        //    // Calculate totals
//        //    var totalItems = cart.Sum(item => item.Quantity);
//        //    var totalPrice = cart.Sum(item => item.Price * item.Quantity);
//        //    var itemQuantity = cart.FirstOrDefault(x => x.MovieId == request.MovieId)?.Quantity ?? 0;

//        //    return Json(new
//        //    {
//        //        success = true,
//        //        totalItems,
//        //        totalPrice,
//        //        itemQuantity
//        //    });
//        //}

//        public class AddToCartRequest
//        {
//            public int MovieId { get; set; }
//            public decimal Price { get; set; }
//        }


//    }


//}

using Lexiflix.Models.Db;
using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lexiflix.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly ICustomerServices _customerServices;

        private const string CartSessionKey = "Cart";

        public OrderController(IOrderServices orderServices, ICustomerServices customerServices)
        {
            _orderServices = orderServices;
            _customerServices = customerServices;
        }

        public IActionResult Index()
        {
            var orders = _orderServices.GetAllOrders();
            return View(orders);
        }

        public IActionResult OrderDetail()
        {
            var orders = _orderServices.GetAllOrders();
            if (orders == null)
                return NotFound();
            return View(orders);
        }

        public IActionResult Create()
        {
            return View();
        }

        //public IActionResult ViewCart()
        //{
        //    var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
        //    var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
        //    return View(cart);
        //}
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


        //[HttpPost]
        //public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        //{
        //    var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
        //    var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
        //    var existingItem = cart.FirstOrDefault(x => x.MovieId == request.MovieId);

        //    if (existingItem != null)
        //    {
        //        if (request.Action == "increase")
        //        {
        //            existingItem.Quantity++;
        //        }
        //        else if (request.Action == "decrease")
        //        {
        //            if (existingItem.Quantity > 1)
        //            {
        //                existingItem.Quantity--;
        //            }
        //            else
        //            {
        //                cart.Remove(existingItem);
        //            }
        //        }
        //    }

        //    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

        //    // Calculate totals
        //    var totalItems = cart.Sum(item => item.Quantity);
        //    var totalPrice = cart.Sum(item => item.Price * item.Quantity);
        //    var itemQuantity = cart.FirstOrDefault(x => x.MovieId == request.MovieId)?.Quantity ?? 0;

        //    return Json(new
        //    {
        //        success = true,
        //        totalItems,
        //        totalPrice,
        //        itemQuantity
        //    });
        //}
        //[HttpPost]
        //public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        //{
        //    var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
        //    var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) ?? new List<OrderRow>();
        //    var existingItem = cart.FirstOrDefault(x => x.MovieId == request.MovieId);

        //    if (existingItem != null)
        //    {
        //        if (request.Action == "increase")
        //        {
        //            existingItem.Quantity++;
        //        }
        //        else if (request.Action == "decrease")
        //        {
        //            if (existingItem.Quantity > 1)
        //            {
        //                existingItem.Quantity--;
        //            }
        //            else
        //            {
        //                cart.Remove(existingItem);
        //            }
        //        }
        //    }

        //    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

        //    // Calculate totals
        //    var totalItems = cart.Sum(item => item.Quantity);
        //    var totalPrice = cart.Sum(item => item.Price * item.Quantity);
        //    var itemQuantity = cart.FirstOrDefault(x => x.MovieId == request.MovieId)?.Quantity ?? 0;

        //    return Json(new
        //    {
        //        success = true,
        //        movieId = request.MovieId,
        //        totalItems,
        //        totalPrice,
        //        itemQuantity
        //    });
        //}
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
        public IActionResult Checkout(string email, Customer customer)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
            if (cart.Count == 0)
                return RedirectToAction("ViewCart");

            var existingCustomer = _customerServices.GetCustomerByEmail(email);
            if (existingCustomer == null)
            {
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
            return RedirectToAction("OrderSuccess");
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

        public IActionResult OrderSuccess()
        {
            return View();
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
    }
}