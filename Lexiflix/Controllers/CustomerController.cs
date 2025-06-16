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


        // GET: /Customer/Edit
        public IActionResult Edit(int id)
        {
           var customer = _customerServices.GetCustomerById(id);
          
            return View(customer);
        }



        // POST:  /Customer/Edit
        [HttpPost]
        public IActionResult Edit(Customer editCustomer)
        {
            if (ModelState.IsValid)
            {
                _customerServices.UpdateCustomer(editCustomer);
                return RedirectToAction("AdminIndex", "Customer");
            }

            return View(editCustomer); 
        }




      
        
     //  List all customers in adminindex
 

           public IActionResult AdminIndex()
        {
            var customers = _customerServices.GetAllCustomers();
            return View(customers);
        }


    
    }
}
