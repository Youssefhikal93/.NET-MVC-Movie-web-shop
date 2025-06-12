using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Globalization;

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

    }
}
