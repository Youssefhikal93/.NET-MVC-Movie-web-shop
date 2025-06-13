using Lexiflix.Models;
using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lexiflix.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        public OrderController (IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        public IActionResult Index()
        {
            var orders = _orderServices.GetAlOrders();
            return View(orders);
        }

        
    }
        
    
}
