using CloneDB.Entities;

namespace CloneDB.Models
{
    public class EditMovieViewModel : BasePageViewModel
    {
        public Movie SelectedMovie { get; set; }
        public string SerializedMovie { get; set; }
    }
}