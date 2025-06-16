using Microsoft.AspNetCore.Mvc;
using Lexiflix.Services;
using Lexiflix.Models.Db;

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
        public IActionResult Create()
        {
            return View();
        }

        // POST:  /Customer/Create
        [HttpPost]
        public IActionResult Create(Customer newCustomer)
        {
            if (ModelState.IsValid)
            {

                _customerServices.AddNewCustomer(newCustomer);
                return RedirectToAction("AdminIndex", "Customer");

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
