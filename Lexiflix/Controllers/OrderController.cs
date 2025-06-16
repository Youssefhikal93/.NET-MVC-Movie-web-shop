using Lexiflix.Models;
using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lexiflix.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly ICustomerServices _customerServices;
        public OrderController (IOrderServices orderServices, ICustomerServices customerServices)
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
            var row = new OrderRow();
            row.Movie = _orderServices.GetMovieById(1);
            return View(row);
        }

        public IActionResult ViewCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
            foreach (var row in cart)
            {
                row.Movie = _orderServices.GetMovieById(row.MovieId);
            }
            return View(cart);

        }

        [HttpPost]
        public IActionResult AddToCart(int movieId, decimal price)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
            var existingItem = cart.FirstOrDefault(x => x.MovieId == movieId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new OrderRow { MovieId = movieId, Price = price, Quantity = 1 });

            }
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("ViewCart");


        }
        [HttpPost]
        public IActionResult RemoveFromCart(int movieId)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
            var existingItem = cart.FirstOrDefault(x => x.MovieId == movieId);

            if (existingItem != null)
            {
                if (existingItem.Quantity > 1)
                {
                    existingItem.Quantity--;
                }
                else
                {
                    cart.Remove(existingItem);


                }
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction("ViewCart");
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

        [HttpPost]
        public IActionResult AddCopy(int movieId)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);

            var existingItem = cart.FirstOrDefault(x => x.MovieId == movieId);
            if (existingItem != null)
            {
                existingItem.Quantity++; // increase copies
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("ViewCart");
        }
        [HttpPost]
        public IActionResult RemoveCopy(int movieId)
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            var cart = JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
            var existingItem = cart.FirstOrDefault(x => x.MovieId == movieId);
            if (existingItem != null)

                if (existingItem.Quantity > 1)
                {

                    existingItem.Quantity--; // Reduce quantity
                }
                else
                {
                    cart.Remove(existingItem); // Remove item if only one copy exists
                }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("ViewCart");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
        private List<OrderRow> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart") ?? "[]";
            return JsonConvert.DeserializeObject<List<OrderRow>>(cartJson);
        }

        private void SaveCart(List<OrderRow> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }




    }


}
