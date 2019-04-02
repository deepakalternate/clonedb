using CloneDB.BAL;
using CloneDB.Models;
using Newtonsoft.Json;

namespace CloneDB.Populators
{
    public class EditMovieModel
    {
        private readonly IMovies _movies;
        
        public EditMovieModel(IMovies movies)
        {
            _movies = movies;
        }

        public EditMovieViewModel GetData(int movieId)
        {
            EditMovieViewModel objData = new EditMovieViewModel();
            
            objData.SelectedMovie = _movies.GetMovieDataById(movieId);

            if (objData.SelectedMovie != null)
            {
                objData.Title = string.Format("Edit Movie Details for {0}", objData.SelectedMovie.Title);
                objData.SerializedMovie = JsonConvert.SerializeObject(objData.SelectedMovie);
            }

            return objData;
        }
    }
}