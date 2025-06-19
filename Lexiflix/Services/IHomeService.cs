using Lexiflix.Models;
using Lexiflix.Models.Db;

namespace Lexiflix.Services
{
    public interface IHomeService
    {
        List<MovieWithOrderCount> GetMostPopularMovies();
        List<Movie> GetNewestReleases();
        List<Movie> GetClassicFilms();
        List<Movie> GetBestDeals();
        public TopCustomerViewModel GetTopCustomer();
    }

}
