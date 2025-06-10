using Lexiflix.Data.Db;

using Microsoft.AspNetCore.Mvc;


namespace Lexiflix.Controllers
{
    public class AdminController : Controller
    {
     

        public IActionResult Index()
        {
            return View();
        }
    

    }
}

