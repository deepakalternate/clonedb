using CloneDB.BAL;
using CloneDB.Models;
using CloneDB.Populators;
using Microsoft.AspNetCore.Mvc;

namespace CloneDB.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovies _movies;
        
        public MovieController(IMovies movies)
        {
            _movies = movies;
        }
        
        [Route("/")]
        public IActionResult Index()
        {
            MovieListingModel movieListingModel = new MovieListingModel(_movies);
            MovieListingViewModel pageData = movieListingModel.GetData();
            return View(pageData);
        }
        
        [HttpGet("/addmovies/")]
        public IActionResult AddMovies()
        {
            AddMovieModel movieModel = new AddMovieModel();
            AddMovieViewModel pageData = movieModel.GetData(); 
            return View(pageData);
        }
        
        [HttpGet("/editmovie/{id}")]
        public IActionResult EditMovies([FromRoute]int id)
        {
            EditMovieModel editMovieModel = new EditMovieModel(_movies);
            EditMovieViewModel pageData = editMovieModel.GetData(id);
            
            return View(pageData);
        }
    }
}