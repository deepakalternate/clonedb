using System.Collections.Generic;
using CloneDB.Entities;

namespace CloneDB.Models
{
    public class MovieListingViewModel : BasePageViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
    }
}