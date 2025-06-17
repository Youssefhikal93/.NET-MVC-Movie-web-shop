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


        // GET: /Customer/Edit
        public IActionResult Edit(int id)
        {
           var customer = _customerServices.GetCustomerById(id);
           if (customer == null) return NotFound();
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

        // GET:  /Customer/Delete
        public IActionResult Delete(int id)
        {
            var customer = _customerServices.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer); 
        }

        // POST:  /Customer/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _customerServices.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }

            _customerServices.DeleteCustomer(id);
            return RedirectToAction("AdminIndex", "Customer");
        }

          [HttpGet]
         // GET :  /Customer/Detail
         public IActionResult Detail(int id)
         {
            var customer = _customerServices.GetCustomerById( id);
            if (customer == null)
            {
                return NotFound();
            }
            return View (customer);
         }      
        
     //  List all customers in adminindex
 

           public IActionResult AdminIndex()
        {
            var customers = _customerServices.GetAllCustomers();
            return View(customers);
        }


    
    }
}
