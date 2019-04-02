using System.Collections.Generic;
using CloneDB.Entities;

namespace CloneDB.BAL
{
    public interface IMovies
    {
        IEnumerable<Movie> GetAllMovies();
        bool SaveMovieData(SaveMovieBundle input);
        Movie GetMovieDataById(int movieId);
        bool UpdateMovieData(SaveMovieBundle input);
    }
}