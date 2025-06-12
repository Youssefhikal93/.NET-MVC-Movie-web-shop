using Lexiflix.Data.Db;
using Lexiflix.Models;
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

                var url = $"{baseUrl}?t={Uri.EscapeDataString(movieTitle)}&apikey={apiKey}";
                var response = await _httpClient.GetAsync(url);

              

                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var omdbMovie = JsonSerializer.Deserialize<OmdbMovieResponse>(json, options);


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



                var movie = new Movie
                {
                    Title = omdbMovie.Title,
                    Director = omdbMovie.Director ?? "Unknown",
                    ReleaseYear = releaseYear,
                    Price = price,
                    ImageUrl = omdbMovie.Poster != "N/A" ? omdbMovie.Poster : null
                };

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
    
};
                foreach (var title in titles)
                {
                    await SeedMovieAsync(title);
                }
            }
        }
    }
}