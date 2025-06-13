using Lexiflix.Models;
using Lexiflix.Models.Db;
using Lexiflix.Utils;

namespace Lexiflix.Services
{
    public interface IMovieServices
    {
        List<Movie> GetAllMovies();
        PaginatedList<Movie> GetMovies(string searchString, string sortBy, int pageIndex, int pageSize);

        Movie GetOneMovie(int id);
        MovieUpdateVM GetMovieForEdit(int id);
        void UpdateMovie(MovieUpdateVM updatedMovie);
    }
}