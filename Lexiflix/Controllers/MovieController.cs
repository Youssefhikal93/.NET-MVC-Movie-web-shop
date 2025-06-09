using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
