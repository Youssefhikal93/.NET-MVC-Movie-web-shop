using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Drawing.Printing;
using System.Globalization;
using System.Net;
using Lexiflix.Models;

namespace Lexiflix.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieServices _movieServices;
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 8;

        public MovieController(IMovieServices movieServices)
        {
            _movieServices = movieServices;
        }

        [HttpGet]
        public IActionResult Index(string searchString = "", string sortBy = "latest", int pageNumber = DefaultPage, int pageSize = DefaultPageSize)
        {
            var movies = _movieServices.GetMovies(searchString, sortBy, pageNumber, pageSize);

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortBy;
            ViewData["PageSize"] = pageSize;
            ViewData["ActionName"] = "Index";

            return View(movies);
        }


        [HttpGet]
        [Route("Admin/Movie")]
        public IActionResult AdminIndex(string searchString = "", string sortBy = "latest", int pageNumber = DefaultPage, int pageSize = DefaultPageSize)
        {
            //var movies = _movieServices.GetAllMovies();
            var movies = _movieServices.GetMovies(searchString, sortBy, pageNumber, pageSize);

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortBy;
            ViewData["PageSize"] = pageSize;
            ViewData["ActionName"] = "AdminIndex";
            return View("AdminIndex", movies);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var movie = _movieServices.GetOneMovie(id);
            return View(movie);
        }

        
        public IActionResult AdminCreate()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AdminCreate(Movie newMovie)
        {
            if (ModelState.IsValid)
            {
                _movieServices.AddMovie(newMovie);
                return RedirectToAction("AdminIndex");
            }
            return View();
        }

        //Get: Delet confirmation page
        public IActionResult Delete(int id)

        {
            var movie = _movieServices.GetOneMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);  /*shows Delete.cshtml*/
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)

        {  _movieServices.DeleteMovie(id); /*pass the id*/
            return RedirectToAction("AdminIndex");
        }

         
    }
}
