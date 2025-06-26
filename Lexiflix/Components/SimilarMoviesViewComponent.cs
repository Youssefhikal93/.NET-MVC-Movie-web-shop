using Lexiflix.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lexiflix.Components
{
    public class SimilarMoviesViewComponent:ViewComponent
    {
        private readonly IMovieServices _movieServices;

        public SimilarMoviesViewComponent(IMovieServices movieServices)
        {
            _movieServices = movieServices;
        }

        //public  IViewComponentResult Invoke(int id,string director)
        //{
        //    var similarMovies =  _movieServices.GetSimilarMovies(id, director);
        //    return View(similarMovies);
        //} 
        
        // to get the first genre in the movie
        public  IViewComponentResult Invoke(int id,string director)
        {
            var referenceMovie = _movieServices.GetOneMovie(id);
            var firstGenreName = referenceMovie?.Genres.FirstOrDefault()?.Name;

            var similarMovies = _movieServices.GetSimilarMovies(id, director);

            return View((similarMovies.AsEnumerable(), firstGenreName));
        }
    }
}
