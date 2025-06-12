using Lexiflix.Models;
using Lexiflix.Utils;

namespace Lexiflix.Services
{
    public interface IMovieServices
    {
        List<Movie> GetAllMovies();
        PaginatedList<Movie> GetMovies(string searchString, string sortBy, int pageIndex, int pageSize);
    }
}