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

                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("OMDb API Key is missing from configuration");
                    return;
                }

                var url = $"{baseUrl}?t={Uri.EscapeDataString(movieTitle)}&apikey={apiKey}";

                _logger.LogInformation($"Requesting movie data for: {movieTitle}");
                _logger.LogInformation($"URL: {url}");

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"HTTP request failed with status: {response.StatusCode}");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"API Response: {json}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var omdbMovie = JsonSerializer.Deserialize<OmdbMovieResponse>(json, options);

                if (omdbMovie == null)
                {
                    _logger.LogError("Failed to deserialize OMDb response");
                    return;
                }

                // Fix: Removed the check for 'Response' property as it does not exist in OmdbMovieResponse
                if (!string.IsNullOrEmpty(omdbMovie.Error))
                {
                    _logger.LogError($"OMDb API returned error: {omdbMovie.Error}");
                    return;
                }

                if (string.IsNullOrEmpty(omdbMovie.Title))
                {
                    _logger.LogError("Movie title is empty in API response");
                    return;
                }

                if (_context.Movies.Any(m => m.Title == omdbMovie.Title))
                {
                    _logger.LogInformation($"Movie '{omdbMovie.Title}' already exists, skipping");
                    return;
                }

                var movie = new Movie
                {
                    Title = omdbMovie.Title,
                    Director = omdbMovie.Director ?? "Unknown",
                    ReleaseYear = int.TryParse(omdbMovie.Year, out var year) ? year : 2000,
                    Price = 99.99m,
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
    }
}