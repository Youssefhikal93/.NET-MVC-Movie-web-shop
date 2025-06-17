using Lexiflix.Models.Db;
namespace Lexiflix.Models
{
    public class MovieSectionVM
    {
        public string Title { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
    }
}
