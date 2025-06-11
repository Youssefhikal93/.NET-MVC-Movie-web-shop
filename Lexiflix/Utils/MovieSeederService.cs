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
    "Inception", "The Matrix", "Interstellar", "V for Vendetta", "Fight Club",
    "Pulp Fiction", "Forrest Gump", "The Shawshank Redemption", "The Dark Knight",
    "Avatar", "Titanic", "Jurassic Park", "Star Wars: A New Hope", "The Lord of the Rings: The Fellowship of the Ring",
    "Spider-Man: Into the Spider-Verse", "The Avengers", "Gladiator", "Braveheart",
    "Saving Private Ryan", "The Lion King", "Toy Story", "Spirited Away",
    "Parasite", "Whiplash", "La La Land", "Eternal Sunshine of the Spotless Mind",
    "Arrival", "Blade Runner 2049", "Mad Max: Fury Road", "Dune",
    "Get Out", "Us", "Hereditary", "Midsommar", 
    "A Quiet Place", "Bird Box", "Train to Busan", "Searching",
    "Knives Out", "Glass Onion: A Knives Out Mystery", "Baby Driver", "Scott Pilgrim vs. the World",
    "Back to the Future", "E.T. the Extra-Terrestrial", "Indiana Jones and the Raiders of the Lost Ark",
    "Ghostbusters", "The Goonies", "Stand by Me", "Ferris Bueller's Day Off",
    "The Breakfast Club", "Dirty Dancing", "Top Gun", "Beverly Hills Cop",
    "Coming to America", "Predator", "Alien", "Aliens",
    "Terminator 2: Judgment Day", "Die Hard", "Léon: The Professional", "Seven",
    "Goodfellas", "Casino", "The Departed", "No Country for Old Men",
    "There Will Be Blood", "Fargo", "The Big Lebowski", "Pulp Fiction",
    "Reservoir Dogs", "Kill Bill: Vol. 1", "Kill Bill: Vol. 2", "Django Unchained",
    "Inglourious Basterds", "Once Upon a Time in Hollywood", "The Hateful Eight", "Jackie Brown",
    "True Romance", "Natural Born Killers", "From Dusk Till Dawn", "Sin City",
    "300", "Watchmen", "The Dark Knight Rises", "Joker",
    "Logan", "Deadpool", "Guardians of the Galaxy", "Doctor Strange",
    "Spider-Man: Homecoming", "Black Panther", "Captain Marvel", "Avengers: Endgame",
    "Avengers: Infinity War", "Thor: Ragnarok", "Wonder Woman", "Aquaman",
    "Shazam!", "Birds of Prey", "The Suicide Squad", "Zack Snyder's Justice League",
    "Harry Potter and the Sorcerer's Stone", "Harry Potter and the Chamber of Secrets",
    "Harry Potter and the Prisoner of Azkaban", "Harry Potter and the Goblet of Fire",
    "Harry Potter and the Order of the Phoenix", "Harry Potter and the Half-Blood Prince",
    "Harry Potter and the Deathly Hallows – Part 1", "Harry Potter and the Deathly Hallows – Part 2",
    "Fantastic Beasts and Where to Find Them", "Fantastic Beasts: The Crimes of Grindelwald",
    "The Hobbit: An Unexpected Journey", "The Hobbit: The Desolation of Smaug",
    "The Hobbit: The Battle of the Five Armies", "The Lord of the Rings: The Two Towers",
    "The Lord of the Rings: The Return of the King", "Avatar: The Way of Water",
    "Top Gun: Maverick", "Dune: Part Two", "Oppenheimer", "Barbie",
    "Past Lives", "Spider-Man: Across the Spider-Verse", "Poor Things", "Anatomy of a Fall",
    "The Holdovers", "Killers of the Flower Moon", "Maestro", "Napoleon",
    "Wonka", "Migration", "Anyone But You", "Madame Web",
    "Argylle", "Kung Fu Panda 4", "Ghostbusters: Frozen Empire", "Godzilla x Kong: The New Empire"
};
                foreach (var title in titles)
                {
                    await SeedMovieAsync(title);
                }
            }
        }
    }
}