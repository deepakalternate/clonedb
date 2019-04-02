using CloneDB.BAL;
using CloneDB.Models;

namespace CloneDB.Populators
{
    public class MovieListingModel
    {
        private readonly IMovies _movies;
        public MovieListingModel(IMovies movies)
        {
            _movies = movies;
        }

        public MovieListingViewModel GetData()
        {
            MovieListingViewModel pageData = new MovieListingViewModel();

            pageData.Title = "Movie Listing";
            pageData.Movies = _movies.GetAllMovies();

            return pageData;
        }
    }
}