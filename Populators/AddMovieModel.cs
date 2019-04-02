using CloneDB.Models;

namespace CloneDB.Populators
{
    public class AddMovieModel
    {
        public AddMovieViewModel GetData()
        {
            AddMovieViewModel pgObj = new AddMovieViewModel();

            pgObj.Title = "Add Movie";
            
            return pgObj;
        }
    }
}