using Lexiflix.Models;
using Lexiflix.Models.Db;

namespace Lexiflix.Services
{
    public interface IHomeService
    {
        //List<MovieViewModel> GetMostPopularMovies();
        List<Movie> GetNewestReleases();
        List<Movie> GetClassicFilms();
        List<Movie> GetBestDeals();
        //TopCustomerViewModel GetTopCustomer();
    }

}
