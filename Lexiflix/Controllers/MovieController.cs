using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lexiflix.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieServices _movieServices;
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 6;

        public MovieController(IMovieServices movieServices)
        {
            _movieServices = movieServices;
        }

        //[HttpGet]
        //public IActionResult Index(int pageNumber = DefaultPage , int pageSize = DefaultPageSize)
        //{
        //    var movies = _movieServices.GetMoviesPaginated(pageNumber, pageSize);
        //    return View(movies);
        //}
        [HttpGet]
        public IActionResult Index(string searchString = "", string sortBy = "latest", int pageNumber = DefaultPage, int pageSize = DefaultPageSize)
        {
            var movies = _movieServices.GetMovies(searchString, sortBy, pageNumber, pageSize);

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortBy;
            ViewData["PageSize"] = pageSize;

            return View(movies);
        }

        //[HttpGet]
        //public IActionResult Search(string searchString, int pageNumber = DefaultPage, int pageSize = DefaultPageSize)
        //{
        //    var movies = _movieServices.SearchMovies(searchString, pageNumber,pageSize);
        //    SetViewData(searchString: searchString);
        //    return View("Index", movies);
        //}

        //[HttpGet]
        //public IActionResult SortByAmount(int pageNumber = DefaultPage, int pageSize = DefaultPageSize)
        //{
        //    var movies = _movieServices.GetMoviesSortedByPrice(pageNumber, pageSize);
        //    SetViewData(sort: "price");
        //    return View("Index", movies);
        //}

        //[HttpGet]
        //public IActionResult SortByYear(int pageNumber = DefaultPage, int pageSize = DefaultPageSize)
        //{
        //    var movies = _movieServices.GetMoviesSortedByYear(pageNumber, pageSize);
        //    SetViewData(sort: "year");
        //    return View("Index", movies);
        //}

        [HttpGet]
        [Route("Admin/Movie")]
        public IActionResult AdminIndex()
        {
            var movies = _movieServices.GetAllMovies();
            return View("AdminIndex", movies);
        }

        //private void SetViewData(string? searchString = null, string? sort = null)
        //{
        //    if (!string.IsNullOrWhiteSpace(searchString))
        //        ViewData["CurrentFilter"] = searchString;

        //    if (!string.IsNullOrWhiteSpace(sort))
        //        ViewData["CurrentSort"] = sort;
        //}
    }
}
