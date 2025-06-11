using Lexiflix.Models;
using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lexiflix.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private int movieId;
        private const string CartSessionKey = "Cart";

        public OrderController (IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        public IActionResult Index()
        {
            var orders = _orderServices.GetAlOrders();
            return View(orders);
        }


        public IActionResult ViewCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = cartJson != null ? JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) : new List<OrderRow>();
            return View(cart);
            
        }

        [HttpPost]
        public IActionResult AddToCart(int movieId, decimal price)
        {
            var cartJson = HttpContext.Session.GetString("cart");
            var cart = cartJson != null ? JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) : new List<OrderRow>();
            var existingItem = cart.FirstOrDefault(c => c.MovieId == movieId );
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

        public IActionResult RemoveFromCart(int movieId)
        {
            var cartJson = HttpContext.Session.GetString("cart");
            var cart = cartJson != null ? JsonConvert.DeserializeObject<List<OrderRow>>(cartJson) : new List<OrderRow>();
            var existingItem = cart.FirstOrDefault( c => c.MovieId == movieId );
            if (existingItem != null) 
            {
                if (existingItem.Quantity > 1 )
                {
                    existingItem.Quantity--;
                }
                else
                {
                    cart.Remove(existingItem);
                }
            }
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            return View();
        }



        
    }
        
    
}
