using Lexiflix.Data.Db;

using Microsoft.AspNetCore.Mvc;


namespace Lexiflix.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                TempData["Authenticated"] = true;
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }
        public IActionResult Index()
        {
            if (TempData["Authenticated"] == null || (bool)TempData["Authenticated"] != true)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    

    }
}

