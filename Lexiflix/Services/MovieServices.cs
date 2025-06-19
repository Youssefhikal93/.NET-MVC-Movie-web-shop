using Lexiflix.Data.Db;
using Lexiflix.Models;
using Lexiflix.Models.Db;
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
        public IEnumerable<Movie> SearchMovies(string query)
        {
            return _db.Movies
                .Where(m => m.Title.Contains(query))
                //.Take(20) 
                .ToList();
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
                    m.Actors.Any(a => a.Name.Contains(searchString))||
                    m.Genres.Any(g=>g.Name.Contains(searchString)));
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
            var movie = GetBaseQuery()
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

                // Pre-select current actors and genres
                SelectedGenreIds = movie.Genres.Select(g => g.Id).ToList(),
                AvailableGenres = _db.Genres.OrderBy(g => g.Name).ToList(),

                //Update Actors(many - to - many)
                SelectedActorIds = movie.Actors.Select(a => a.Id).ToList(),
                AvailableActors = _db.Actors
                        .Where(a => a.Movies.Any(m => m.Id == id))
                        .ToList(),

            };
        }

        public void UpdateMovie(MovieUpdateVM vm)
        {
            var movie = GetBaseQuery().FirstOrDefault(m => m.Id == vm.Id);
            if (movie == null) return;

            // Update basic fields only if they have values
            if (vm.Title != null) movie.Title = vm.Title;
            if (vm.Director != null) movie.Director = vm.Director;
            if (vm.ReleaseYear.HasValue) movie.ReleaseYear = vm.ReleaseYear.Value;
            if (vm.Price.HasValue) movie.Price = vm.Price.Value;
            if (vm.ImageUrl != null) movie.ImageUrl = vm.ImageUrl;
            if (vm.Plot != null) movie.Plot = vm.Plot;
            if (vm.Genre != null) movie.Genre = vm.Genre;
            if (vm.Runtime.HasValue) movie.Runtime = vm.Runtime.Value;
            if (vm.Rating != null) movie.Rating = vm.Rating;
            if (vm.ImdbRating.HasValue) movie.ImdbRating = vm.ImdbRating.Value;

            // Update Actors (only if there are selected actors or custom actors)
            if (vm.SelectedActorIds != null || vm.CustomActors != null)
            {
                movie.Actors.Clear();

                if (vm.SelectedActorIds != null && vm.SelectedActorIds.Any())
                {
                    var selectedActors = _db.Actors
                        .Where(a => vm.SelectedActorIds.Contains(a.Id))
                        .ToList();
                    movie.Actors.AddRange(selectedActors);
                }

                if (!string.IsNullOrWhiteSpace(vm.CustomActors))
                {
                    var customActorNames = vm.CustomActors
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(name => name.Trim());

                    foreach (var actorName in customActorNames)
                    {
                        var existingActor = _db.Actors
                            .FirstOrDefault(a => a.Name.ToLower() == actorName.ToLower());

                        if (existingActor != null)
                        {
                            if (!movie.Actors.Any(a => a.Id == existingActor.Id))
                            {
                                movie.Actors.Add(existingActor);
                            }
                        }
                        else
                        {
                            var newActor = new Actor { Name = actorName };
                            _db.Actors.Add(newActor);
                            movie.Actors.Add(newActor);
                        }
                    }
                }
            }

            // Update Genres (only if there are selected genres)
            if (vm.SelectedGenreIds != null)
            {
                movie.Genres.Clear();
                if (vm.SelectedGenreIds.Any())
                {
                    var selectedGenres = _db.Genres
                        .Where(g => vm.SelectedGenreIds.Contains(g.Id))
                        .ToList();
                    movie.Genres.AddRange(selectedGenres);
                }
            }

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

        public void AddMovie(Movie movie)
        {
            _db.Movies.Add(movie);
            _db.SaveChanges();

        }

        public void AddMovieWithActorsAndGenres(Movie movie, string actorNames, string genreNames)
        {
            var actorList = actorNames.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(name => name.Trim()).Distinct();

            var genreList = genreNames.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(name => name.Trim()).Distinct();

            foreach (var actorName in actorList)
            {
                var actor = _db.Actors.FirstOrDefault(a => a.Name == actorName)
                            ?? new Actor { Name = actorName };

                movie.Actors.Add(actor);
            }

            foreach (var genreName in genreList)
            {
                var genre = _db.Genres.FirstOrDefault(g => g.Name == genreName)
                            ?? new Genre { Name = genreName };

                movie.Genres.Add(genre);
            }

            _db.Movies.Add(movie);
            _db.SaveChanges();
        }

        public List<Movie> GetMovies()
        {
            return _db.Movies.ToList();
        }

        public void DeleteMovie(int id)

        {
            var movie = _db.Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                _db.Movies.Remove(movie);
                _db.SaveChanges();
            }
        }

        
    }
}
