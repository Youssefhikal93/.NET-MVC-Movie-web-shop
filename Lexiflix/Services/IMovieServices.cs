using Lexiflix.Models;
using Lexiflix.Models.Db;

namespace Lexiflix.Services
{
    public interface IMovieServices
    {
        List<Movie> GetAllMovies();
        PaginatedList<Movie> GetMovies(string searchString, string sortBy, int pageIndex, int pageSize);

        Movie GetOneMovie(int id);

        MovieUpdateVM GetMovieForEdit(int id);
        void UpdateMovie(MovieUpdateVM updatedMovie);

        void AddMovie(Movie movie);
        void DeleteMovie(int id);
        IEnumerable<Movie> SearchMovies(string query);

             
        void AddMovieWithActorsAndGenres(Movie movie, string actorNames, string genreNames);

        public List<Movie> GetSimilarMovies(int id,string director);


    }
}