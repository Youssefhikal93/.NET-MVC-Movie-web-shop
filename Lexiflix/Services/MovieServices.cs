using Lexiflix.Data.Db;
using Lexiflix.Models;
using Lexiflix.Utils;
using Microsoft.EntityFrameworkCore;

namespace Lexiflix.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly MovieDbContext _db;
      

        public MovieServices(MovieDbContext db)
        {
            _db = db;
        }

       
        public List<Movie> GetAllMovies()
        {
            return GetBaseQuery().ToList();
        }

        
        public PaginatedList<Movie> GetMovies(string searchString, string sortBy, int pageIndex, int pageSize)
        {
            var query = GetBaseQuery();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m =>
                    m.Title.Contains(searchString) ||
                    m.Director.Contains(searchString) ||
                    m.Actors.Any(a => a.Name.Contains(searchString)));
            }

            // Apply sorting
            query = ApplySorting(query, sortBy);

            return Paginate(query, pageIndex, pageSize);
        }

        private IQueryable<Movie> ApplySorting(IQueryable<Movie> query, string sortBy)
        {
            return sortBy.ToLower() switch
            {
                "latest" => query.OrderByDescending(m => m.ReleaseYear),
                "oldest" => query.OrderBy(m => m.ReleaseYear),
                "pricehigh" => query.OrderByDescending(m => m.Price),
                "pricelow" => query.OrderBy(m => m.Price),
                "title" => query.OrderBy(m => m.Title),
                _ => query.OrderByDescending(m => m.ReleaseYear) // Default to latest
            };
        }

        private IQueryable<Movie> GetBaseQuery() =>
           _db.Movies.Include(m => m.Actors).AsQueryable();

        private PaginatedList<Movie> Paginate(IQueryable<Movie> query, int pageIndex, int pageSize) =>
            PaginatedList<Movie>.Create(query, pageIndex, pageSize);

    }
}
