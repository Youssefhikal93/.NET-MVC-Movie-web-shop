using Lexiflix.Models;
using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;
using Lexiflix.Models.Db;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Lexiflix.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieServices _movieServices;
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 12;

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
            ViewData["ControllerName"] = "Movie";

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
        public IActionResult Details(int id, string origin = "Index", string originController = "Movie")
        {
            var movie = _movieServices.GetOneMovie(id);
            ViewData["ActionName"] = origin;
            ViewData["ControllerName"] = originController;

            return View(movie);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movieVm = _movieServices.GetMovieForEdit(id);
            if (movieVm == null) return NotFound();
            return View(movieVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, MovieUpdateVM model)
        {

            if (!ModelState.IsValid)
                return View(model);

            _movieServices.UpdateMovie(model);
            return RedirectToAction("AdminIndex");
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
                string actorNames = Request.Form["ActorNames"];
                string genreNames = Request.Form["GenreNames"];
                
                _movieServices.AddMovieWithActorsAndGenres(newMovie, actorNames, genreNames);
                TempData["SuccessMessage"] = "Movie added successfully!";
                return RedirectToAction("AdminIndex");
            }
            return View();
        }

      
        //Get: Delet confirmation page
        public IActionResult Delete(int id)

        {
           return View();  /*shows Delete.cshtml*/
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)

        {
            try
            {
                _movieServices.DeleteMovie(id); /*pass the id*/
                TempData["SuccessMessage"] = "The movie has been deleted successfully.";
                return RedirectToAction("AdminIndex");
            }
            catch (Exception)
            {
                TempData["SuccessMessage"] = "The movie cannot be deleted.";
                return RedirectToAction("AdminIndex");

            }

        }


        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
                return Json(new List<object>()); 

            var results = _movieServices
                .SearchMovies(query) 
                .Select(movie => new
                {
                    id = movie.Id,
                    title = movie.Title,
                    posterUrl = movie.ImageUrl 
                })
                .ToList();

            return Json(results);
        }

    }
}
