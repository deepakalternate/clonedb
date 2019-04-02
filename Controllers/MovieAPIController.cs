using CloneDB.BAL;
using CloneDB.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CloneDB.Controllers
{
    public class MovieAPIController : ControllerBase
    {
        private readonly IMovies _movies;

        public MovieAPIController(IMovies movies)
        {
            _movies = movies;
        }
        
        [HttpPost]
        [Route("api/movie/save")]
        public ActionResult<bool> SaveMovieDetails([FromBody]SaveMovieBundle input)
        {
            if (input != null)
            {
                bool isSaved = _movies.SaveMovieData(input);
                return Ok(isSaved);
            }
            else
            {
                return BadRequest();
            }
            
        }
        
        [HttpPost]
        [Route("api/movie/update")]
        public ActionResult<bool> UpdateMovieDetails([FromBody]SaveMovieBundle input)
        {
            if (input != null)
            {
                bool isSaved = _movies.UpdateMovieData(input);
                return Ok(isSaved);
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}