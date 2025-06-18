using Lexiflix.Models.Db;
namespace Lexiflix.Models
{
    public class MovieSectionVM
    {
        public string Title { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<MovieWithOrderCount> MoviesWithOrderCount { get; set; }
        public bool ShowOrderCount { get; set; } = false;
    }
}
