using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Globalization;

namespace Lexiflix.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieServices _movieServices;

        public MovieController( IMovieServices movieServices)
        {
            _movieServices = movieServices;
        }
        public IActionResult Index()
        {
            var movies = _movieServices.GetAllMovies();
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
            ViewData["PageSize"] = pageSize; return View("AdminIndex", movies);
        }
    }
}
