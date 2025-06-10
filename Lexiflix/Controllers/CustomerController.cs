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

                _customerService.AddNewCustomer(newCustomer);
                return RedirectToAction("Index");
            }
            return View();
    

           
        }

        //  List all customers
       /* public IActionResult Index()
        {
            List<Customer> customerList= _customerService.ListCustomers();
            return View(customerList);
        } */
    }
}
