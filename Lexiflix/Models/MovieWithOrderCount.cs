using Lexiflix.Models.Db;

namespace Lexiflix.Models
{
    public class MovieWithOrderCount
    {
        public Movie Movie { get; set; }
        public int OrderCount { get; set; }
    }
}
