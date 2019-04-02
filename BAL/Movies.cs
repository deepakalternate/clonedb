using System;
using System.Collections.Generic;
using CloneDB.DAL;
using CloneDB.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace CloneDB.BAL
{
    public class Movies : IMovies
    {
        private readonly IMoviesRepository _movieRepo;
        
        public Movies(IMoviesRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }
        
        /// <summary>
        /// Business layer function to get all movies
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Movie> GetAllMovies()
        {
            IEnumerable<Movie> result = null;
            try
            {
                result = _movieRepo.GetAllMovies();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }
        
        /// <summary>
        /// Business layer function to save all the data related to a movie (including the actor-movie mapping)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool SaveMovieData(SaveMovieBundle input)
        {
            bool isDataSaved = false;
            int movieId = 0;

            try
            {
                if (input != null)
                {
                    movieId = _movieRepo.SaveMovie(input);

                    if (movieId > 0 && input.ActorIds != null && input.ActorIds.Any())
                    {
                        foreach (int actorId in input.ActorIds)
                        {
                            _movieRepo.SaveActorMovieMapping(movieId, actorId);
                        }
                    }

                    isDataSaved = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return isDataSaved;
        }
        
        /// <summary>
        /// Business layer function to get data for a movie by its id
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public Movie GetMovieDataById(int movieId)
        {
            Movie result = null;
            try
            {
                result = _movieRepo.GetMovieDataById(movieId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        /// <summary>
        /// Business layer function to update data of movie
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool UpdateMovieData(SaveMovieBundle input)
        {
            bool isDataSaved = false;
            
            try
            {
                if (input != null)
                {
                    int movieId = input.Id;

                    if (movieId > 0)
                    {
                        isDataSaved = _movieRepo.UpdateMovie(input);
                        
                        _movieRepo.RemoveActorMovieMapping(movieId);
                        
                        if (input.ActorIds != null && input.ActorIds.Any())
                        {
                            foreach (int actorId in input.ActorIds)
                            {
                                _movieRepo.SaveActorMovieMapping(movieId, actorId);
                            }
                        }
                    }

                    isDataSaved = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return isDataSaved;
        }
    }
}