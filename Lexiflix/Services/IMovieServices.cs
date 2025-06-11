using Lexiflix.Models;
using Lexiflix.Utils;

namespace Lexiflix.Services
{
    public interface IMovieServices
    {
        List<Movie> GetAllMovies();
        //PaginatedList<Movie> GetMoviesPaginated(int pageIndex, int pageSize );
        //PaginatedList<Movie> SearchMovies(string searchString, int pageIndex, int pageSize);
        //PaginatedList<Movie> GetMoviesSortedByPrice(int pageIndex, int pageSize );
        //PaginatedList<Movie> GetMoviesSortedByYear(int pageIndex, int pageSize );
        PaginatedList<Movie> GetMovies(string searchString, string sortBy, int pageIndex, int pageSize);
    }
}