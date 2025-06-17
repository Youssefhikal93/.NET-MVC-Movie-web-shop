//using Lexiflix.Data.Db;
//using Lexiflix.Models;
//using System.Text.Json;

//namespace Lexiflix.Utils
//{
//    public class MovieSeederService
//    {
//        private readonly MovieDbContext _context;
//        private readonly HttpClient _httpClient;
//        private readonly IConfiguration _config;
//        private readonly ILogger<MovieSeederService> _logger;

//        public MovieSeederService(MovieDbContext context, HttpClient httpClient, IConfiguration config, ILogger<MovieSeederService> logger)
//        {
//            _context = context;
//            _httpClient = httpClient;
//            _config = config;
//            _logger = logger;
//        }

//        public async Task SeedMovieAsync(string movieTitle)
//        {
//            try
//            {
//                var apiKey = _config["OMDb:ApiKey"];
//                var baseUrl = _config["OMDb:BaseUrl"];

//                var url = $"{baseUrl}?t={Uri.EscapeDataString(movieTitle)}&apikey={apiKey}";
//                var response = await _httpClient.GetAsync(url);



//                var json = await response.Content.ReadAsStringAsync();

//                var options = new JsonSerializerOptions
//                {
//                    PropertyNameCaseInsensitive = true
//                };

//                var omdbMovie = JsonSerializer.Deserialize<OmdbMovieResponse>(json, options);


//                // Price logic 
//                var releaseYear = int.TryParse(omdbMovie.Year, out var year) ? year : 2000;
//                decimal price;
//                if (releaseYear >= DateTime.Now.Year - 10)
//                    price = 149.99m; 
//                else if (releaseYear >= DateTime.Now.Year - 20)
//                    price = 99.99m;  
//                else if (releaseYear >= 1980)
//                    price = 49.99m;  
//                else
//                    price = 19.99m;



//                var movie = new Movie
//                {
//                    Title = omdbMovie.Title,
//                    Director = omdbMovie.Director ?? "Unknown",
//                    ReleaseYear = releaseYear,
//                    Price = price,
//                    ImageUrl = omdbMovie.Poster != "N/A" ? omdbMovie.Poster : null
//                };

//                if (!string.IsNullOrEmpty(omdbMovie.Actors) && omdbMovie.Actors != "N/A")
//                {
//                    var actorNames = omdbMovie.Actors.Split(',', StringSplitOptions.RemoveEmptyEntries)
//                                                     .Select(a => a.Trim())
//                                                     .Where(a => !string.IsNullOrEmpty(a));

//                    foreach (var name in actorNames)
//                    {
//                        var actor = _context.Actors.FirstOrDefault(a => a.Name == name);
//                        if (actor == null)
//                        {
//                            actor = new Actor { Name = name };
//                            _context.Actors.Add(actor);
//                        }
//                        movie.Actors.Add(actor);
//                    }  
//                }

//                _context.Movies.Add(movie);
//                await _context.SaveChangesAsync();

//                _logger.LogInformation($"Successfully seeded movie: {movie.Title}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error seeding movie '{movieTitle}': {ex.Message}");
//            }
//        }

//        public async Task SeedInitialMoviesAsync()
//        {
//            if (!_context.Movies.Any())
//            {
//                var titles = new[] {
//    "The Shawshank Redemption",
//    "The Godfather",
//    "The Dark Knight",

//};
//                foreach (var title in titles)
//                {
//                    await SeedMovieAsync(title);
//                }
//            }
//        }
//    }
//}

using Lexiflix.Data.Db;
using Lexiflix.Models;
using Lexiflix.Models.Db;
using System.Text.Json;

namespace Lexiflix.Utils
{
    public class MovieSeederService
    {
        private readonly MovieDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<MovieSeederService> _logger;

        public MovieSeederService(MovieDbContext context, HttpClient httpClient, IConfiguration config, ILogger<MovieSeederService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task SeedMovieAsync(string movieTitle)
        {
            try
            {
                var apiKey = _config["OMDb:ApiKey"];
                var baseUrl = _config["OMDb:BaseUrl"];
                var url = $"{baseUrl}?t={Uri.EscapeDataString(movieTitle)}&apikey={apiKey}&plot=full";

                var response = await _httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var omdbMovie = JsonSerializer.Deserialize<OmdbMovieResponse>(json, options);

                if (!string.IsNullOrEmpty(omdbMovie?.Error))
                {
                    _logger.LogWarning($"OMDB API returned error for '{movieTitle}': {omdbMovie.Error}");
                    return;
                }

                // Check if movie already exists
                if (_context.Movies.Any(m => m.Title == omdbMovie.Title))
                {
                    _logger.LogInformation($"Movie '{omdbMovie.Title}' already exists in database");
                    return;
                }

                // Price logic 
                var releaseYear = int.TryParse(omdbMovie.Year, out var year) ? year : 2000;
                decimal price;
                if (releaseYear >= DateTime.Now.Year - 10)
                    price = 149.99m;
                else if (releaseYear >= DateTime.Now.Year - 20)
                    price = 99.99m;
                else if (releaseYear >= 1980)
                    price = 49.99m;
                else
                    price = 19.99m;

                // Parse runtime (format: "142 min" -> 142)
                int? runtime = null;
                if (!string.IsNullOrEmpty(omdbMovie.Runtime) && omdbMovie.Runtime != "N/A")
                {
                    var runtimeStr = omdbMovie.Runtime.Replace(" min", "").Trim();
                    if (int.TryParse(runtimeStr, out var runtimeValue))
                    {
                        runtime = runtimeValue;
                    }
                }

                // Parse IMDB rating
                decimal? imdbRating = null;
                if (!string.IsNullOrEmpty(omdbMovie.ImdbRating) && omdbMovie.ImdbRating != "N/A")
                {
                    if (decimal.TryParse(omdbMovie.ImdbRating, out var rating))
                    {
                        imdbRating = rating;
                    }
                }

                var movie = new Movie
                {
                    Title = omdbMovie.Title,
                    Director = omdbMovie.Director != "N/A" ? omdbMovie.Director : "Unknown",
                    ReleaseYear = releaseYear,
                    Price = price,
                    ImageUrl = omdbMovie.Poster != "N/A" ? omdbMovie.Poster : null,
                    Plot = omdbMovie.Plot != "N/A" ? omdbMovie.Plot : null,
                    Runtime = runtime,
                    Rating = omdbMovie.Rated != "N/A" ? omdbMovie.Rated : null,
                    ImdbRating = imdbRating
                };

                // Handle genres (comma-separated string from OMDB)
                if (!string.IsNullOrEmpty(omdbMovie.Genre) && omdbMovie.Genre != "N/A")
                {
                    var genreNames = omdbMovie.Genre.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(g => g.Trim())
                                                   .Where(g => !string.IsNullOrEmpty(g));

                    foreach (var name in genreNames)
                    {
                        var genre = _context.Genres.FirstOrDefault(g => g.Name == name);
                        if (genre == null)
                        {
                            genre = new Genre { Name = name };
                            _context.Genres.Add(genre);
                        }
                        movie.Genres.Add(genre);
                    }
                }

                // Handle actors
                if (!string.IsNullOrEmpty(omdbMovie.Actors) && omdbMovie.Actors != "N/A")
                {
                    var actorNames = omdbMovie.Actors.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(a => a.Trim())
                                                     .Where(a => !string.IsNullOrEmpty(a));

                    foreach (var name in actorNames)
                    {
                        var actor = _context.Actors.FirstOrDefault(a => a.Name == name);
                        if (actor == null)
                        {
                            actor = new Actor { Name = name };
                            _context.Actors.Add(actor);
                        }
                        movie.Actors.Add(actor);
                    }
                }

                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully seeded movie: {movie.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error seeding movie '{movieTitle}': {ex.Message}");
            }
        }

        public async Task SeedInitialMoviesAsync()
        {
            if (!_context.Movies.Any())
            {
                var titles = new[] {
    "The Shawshank Redemption",
    "The Godfather",
    "The Dark Knight",
    "Pulp Fiction",
    "Forrest Gump",
    "The Matrix",
    "Goodfellas",
    "Inception",
    "Fight Club",
    "The Lord of the Rings: The Fellowship of the Ring",
    "The Lord of the Rings: The Return of the King",
    "The Lord of the Rings: The Two Towers",
    "Star Wars: Episode V - The Empire Strikes Back",
    "Interstellar",
    "The Silence of the Lambs",
    "Schindler's List",
    "Parasite",
    "The Green Mile",
    "Se7en",
    "The Usual Suspects",
    "Leon: The Professional",
    "Saving Private Ryan",
    "The Prestige",
    "Gladiator",
    "The Departed",
    "The Pianist",
    "Whiplash",
    "Back to the Future",
    "Alien",
    "The Lion King",
    "Terminator 2: Judgment Day",
    "American History X",
    "The Shining",
    "Spirited Away",
    "The Godfather: Part II",
    "Casablanca",
    "Avengers: Infinity War",
    "Avengers: Endgame",
    "The Dark Knight Rises",
    "Toy Story",
    "Toy Story 3",
    "Jurassic Park",
    "The Truman Show",
    "The Sixth Sense",
    "The Social Network",
    "Inglourious Basterds",
    "Django Unchained",
    "The Wolf of Wall Street",
    "The Big Lebowski",
    "Eternal Sunshine of the Spotless Mind",
    "Memento",
    "Gone with the Wind",
    "Citizen Kane",
    "2001: A Space Odyssey",
    "A Clockwork Orange",
    "Blade Runner",
    "Blade Runner 2049",
    "The Grand Budapest Hotel",
    "La La Land",
    "No Country for Old Men",
    "There Will Be Blood",
    "The Revenant",
    "Mad Max: Fury Road",
    "The Exorcist",
    "Joker",
    "Heat",
    "Scarface",
    "Taxi Driver",
    "Raging Bull",
    "Apocalypse Now",
    "The Good, the Bad and the Ugly",
    "Once Upon a Time in the West",
    "Unforgiven",
    "Raiders of the Lost Ark",
    "Indiana Jones and the Last Crusade",
    "Die Hard",
    "The Terminator",
    "Rocky",
    "Titanic",
    "The Incredibles",
    "Up",
    "Wall-E",
    "Finding Nemo",
    "The Jungle Book",
    "Beauty and the Beast",
    "Frozen",
    "The Avengers",
    "Guardians of the Galaxy",
    "Black Panther",
    "Logan",
    "Deadpool",
    "The Hangover",
    "Superbad",
    "Anchorman: The Legend of Ron Burgundy",
    "The Intouchables",
    "Amelie",
    "Oldboy",
    "Trainspotting",
    "Requiem for a Dream",
    "The Lives of Others",
    "Pan's Labyrinth",
    "City of God",
    "Amadeus"
};

                foreach (var title in titles)
                {
                    await SeedMovieAsync(title);
                }
            }
        }
    }
}