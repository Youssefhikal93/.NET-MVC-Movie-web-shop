using Lexiflix.Data.Db;
using Lexiflix.Models;
using Microsoft.EntityFrameworkCore;

namespace Lexiflix.Services
{
    public class MovieServices :IMovieServices
    {
        private readonly MovieDbContext _db;

        public MovieServices(MovieDbContext db)
        {
            _db = db;
        }

        public List<Movie> GetAllMovies()
        {
            var movieList = _db.Movies.Include(m => m.Actors).ToList();
            
            return movieList;
        }
    }
}
