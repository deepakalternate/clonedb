using System.Collections.Generic;
using CloneDB.Entities;

namespace CloneDB.DAL
{
    public interface IMoviesRepository
    {
        IEnumerable<Movie> GetAllMovies();
        int SaveMovie(SaveMovieBundle input);
        bool SaveActorMovieMapping(int movieId, int actorId);
        Movie GetMovieDataById(int movieId);
        void RemoveActorMovieMapping(int movieId);
        bool UpdateMovie(SaveMovieBundle input);
    }
}