using Lexiflix.Models.Db;

namespace Lexiflix.Models
{
    public class HomeVM
    {
        public List<MovieWithOrderCount> MostPopularMovies { get; set; } = new List<MovieWithOrderCount>();
        public TopCustomerViewModel TopCustomer { get; set; }
        public List<Movie> NewestReleases { get; set; } = new List<Movie>();
        public List<Movie> ClassicFilms { get; set; } = new List<Movie>();
        public List<Movie> BestDeals { get; set; } = new List<Movie>();
    }

    

}
