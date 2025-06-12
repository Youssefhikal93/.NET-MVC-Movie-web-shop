using Microsoft.AspNetCore.Mvc;
using Lexiflix.Models;
using Lexiflix.Services;

namespace Lexiflix.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerServices _customerServices;

        public CustomerController(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        // GET: /Customer/addCustomer
        public IActionResult AddCustomer()
        {
            return View();
        }

        // POST:  /Customer/addCustomer
        [HttpPost]
        public IActionResult AddCustomer(Customer newCustomer)
        {
            if (ModelState.IsValid)
            {

                _customerServices.AddNewCustomer(newCustomer);
                return RedirectToAction("Index");
            }
            return View();
        }
        
     //  List all customers in adminindex
 

           public IActionResult AdminIndex()
        {
            var customers = _customerServices.GetAllCustomers();
            return View(customers);
        }


    
    }
}
