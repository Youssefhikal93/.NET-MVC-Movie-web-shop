using Lexiflix.Data.Db;
using Lexiflix.Models;
using Lexiflix.Models.Db;
using Lexiflix.Utils;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

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


        public Movie? GetOneMovie(int id)
        {
            var query = GetBaseQuery();
            var movie = query.FirstOrDefault(m => m.Id == id);
            return movie;
        }


        public MovieUpdateVM GetMovieForEdit(int id)
        {
            var movie = _db.Movies
                .Include(m => m.Actors)
                .Include(m => m.Genres)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null) return null;

            return new MovieUpdateVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Director = movie.Director,
                ReleaseYear = movie.ReleaseYear,
                Price = movie.Price,
                ImageUrl = movie.ImageUrl,
                Plot = movie.Plot,
                Genre = movie.Genre,
                Runtime = movie.Runtime,
                Rating = movie.Rating,
                ImdbRating = movie.ImdbRating,
                SelectedActorIds = movie.Actors.Select(a => a.Id).ToList(),
                SelectedGenreIds = movie.Genres.Select(g => g.Id).ToList(),
                AvailableActors = _db.Actors.ToList(),
                AvailableGenres = _db.Genres.ToList()
            };
        }

        public void UpdateMovie(MovieUpdateVM vm)
        {
            var movie = _db.Movies
                .Include(m => m.Actors)
                .Include(m => m.Genres)
                .FirstOrDefault(m => m.Id == vm.Id);

            if (movie == null) return;

            // Basic fields
            movie.Title = vm.Title;
            movie.Director = vm.Director;
            movie.ReleaseYear = (int)vm.ReleaseYear;
            movie.Price = (decimal)vm.Price;
            movie.ImageUrl = vm.ImageUrl;
            movie.Plot = vm.Plot;
            movie.Genre = vm.Genre;
            movie.Runtime = vm.Runtime;
            movie.Rating = vm.Rating;
            movie.ImdbRating = vm.ImdbRating;

            // Update Actors (many-to-many)
            movie.Actors.Clear();
            var selectedActors = _db.Actors.Where(a => vm.SelectedActorIds.Contains(a.Id)).ToList();
            movie.Actors.AddRange(selectedActors);

            // Update Genres (many-to-many)
            movie.Genres.Clear();
            var selectedGenres = _db.Genres.Where(g => vm.SelectedGenreIds.Contains(g.Id)).ToList();
            movie.Genres.AddRange(selectedGenres);

            _db.SaveChanges();
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
           _db.Movies.Include(m => m.Actors)
            .Include(m => m.Genres)
            .AsQueryable();

        private PaginatedList<Movie> Paginate(IQueryable<Movie> query, int pageIndex, int pageSize) =>
            PaginatedList<Movie>.Create(query, pageIndex, pageSize);

    }
}
