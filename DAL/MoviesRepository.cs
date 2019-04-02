using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CloneDB.Entities;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace CloneDB.DAL
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly IDbFactory _dbFactory;

        public MoviesRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        
        /// <summary>
        /// Data layer function to get the list of all movies from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Movie> GetAllMovies()
        {
            IEnumerable<Movie> result = null;

            try
            {
                List<Movie> movies = new List<Movie>();
                Dictionary<int, List<Person>> movieActorDict = new Dictionary<int, List<Person>>();
                
                using (MySqlConnection conn = _dbFactory.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "getallmovies";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                    
                        conn.Open();

                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    Movie temp = new Movie
                                    {
                                        Id = Convert.ToInt32(dr["MovieId"]),
                                        Title = Convert.ToString(dr["MovieName"]),
                                        Plot = Convert.ToString(dr["Plot"]),
                                        PosterPath = Convert.ToString(dr["PosterPath"]),
                                        Producer = new Person
                                        {
                                            Id = Convert.ToInt32(dr["ProducerId"]),
                                            Name = Convert.ToString(dr["ProducerName"]),
                                            DOB = Convert.ToDateTime(dr["ProducerDOB"]),
                                            Sex = (Sex) Convert.ToInt32(dr["ProducerSex"]),
                                            Bio = Convert.ToString(dr["ProducerBio"])
                                        },
                                        ReleaseDate = Convert.ToDateTime(dr["ReleaseDate"]),
                                    };
                                    
                                    movies.Add(temp);
                                }
                                
                                // Second result set contains the actor and movie mapping
                                if (dr.NextResult())
                                {
                                    int currentMovie = 0;

                                    while (dr.Read())
                                    {
                                        currentMovie = Convert.ToInt32(dr["MovieId"]);

                                        Person tempActor = new Person
                                        {
                                            Id = Convert.ToInt32(dr["ActorId"]),
                                            Name = Convert.ToString(dr["ActorName"]),
                                            DOB = Convert.ToDateTime(dr["ActorDOB"]),
                                            Sex = (Sex) Convert.ToInt32(dr["ActorSex"]),
                                            Bio = Convert.ToString(dr["ActorBio"])
                                        };

                                        if (movieActorDict.ContainsKey(currentMovie))
                                        {
                                            movieActorDict[currentMovie].Add(tempActor);
                                        }
                                        else
                                        {
                                            List<Person> movieActors = new List<Person>();
                                            movieActors.Add(tempActor);
                                            movieActorDict.Add(currentMovie, movieActors);
                                        }
                                    }
                                }
                            }
                        }
                        
                        conn.Close();
                    }
                }

                // Add actors to Movie list
                foreach (Movie mov in movies)
                {
                    if (movieActorDict.ContainsKey(mov.Id))
                    {
                        mov.Actors = movieActorDict[mov.Id];
                    }
                }
                

                result = movies;

            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        
        /// <summary>
        /// Data layer function to save movie data into database.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int SaveMovie(SaveMovieBundle input)
        {
            int insertedId = 0;
            
            try
            {
                using (MySqlConnection conn = _dbFactory.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "addmovie";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        
                        cmd.Parameters.AddWithValue("par_name", input.MovieTitle);
                        cmd.Parameters.AddWithValue("par_releasedate", input.ReleaseDate.Date);
                        cmd.Parameters.AddWithValue("par_plot", input.Plot);
                        cmd.Parameters.AddWithValue("par_poster", input.PosterPath);
                        cmd.Parameters.AddWithValue("par_producerid", input.ProducerId);
                    
                        conn.Open();

                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    insertedId = Convert.ToInt32(dr["id"]);
                                }
                            }
                        }
                        
                        conn.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }

            return insertedId;
        }
        
        /// <summary>
        /// Data layer function to store actor against a movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="actorId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public bool SaveActorMovieMapping(int movieId, int actorId)
        {
            bool isInserted = false;

            try
            {
                using (MySqlConnection conn = _dbFactory.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "saveactortomovie";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("par_movieid", movieId);
                        cmd.Parameters.AddWithValue("par_actorid", actorId);
                    
                        cmd.Connection = conn;
                    
                        conn.Open();

                        isInserted = cmd.ExecuteNonQuery() > 0;
                        
                        conn.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }
            
            return isInserted;
        }
        
        /// <summary>
        /// Data layer function to get data of a single movie.
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public Movie GetMovieDataById(int movieId)
        {
            Movie movie = null;

            try
            {
                using (MySqlConnection conn = _dbFactory.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "getmoviedatabyid";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("par_movieid", movieId);
                        cmd.Connection = conn;

                        conn.Open();

                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    movie = new Movie()
                                    {
                                        Id = Convert.ToInt32(dr["MovieId"]),
                                        Title = Convert.ToString(dr["MovieName"]),
                                        Plot = Convert.ToString(dr["Plot"]),
                                        PosterPath = Convert.ToString(dr["PosterPath"]),
                                        Producer = new Person
                                        {
                                            Id = Convert.ToInt32(dr["ProducerId"]),
                                            Name = Convert.ToString(dr["ProducerName"]),
                                            DOB = Convert.ToDateTime(dr["ProducerDOB"]),
                                            Sex = (Sex) Convert.ToInt32(dr["ProducerSex"]),
                                            Bio = Convert.ToString(dr["ProducerBio"])
                                        },
                                        ReleaseDate = Convert.ToDateTime(dr["ReleaseDate"]),
                                    };
                                }
                                
                                if (dr.NextResult())
                                {
                                    
                                    IList<Person> actors = new List<Person>();
                                    while (dr.Read())
                                    {
                                        Person tempActor = new Person
                                        {
                                            Id = Convert.ToInt32(dr["ActorId"]),
                                            Name = Convert.ToString(dr["ActorName"]),
                                            DOB = Convert.ToDateTime(dr["ActorDOB"]),
                                            Sex = (Sex) Convert.ToInt32(dr["ActorSex"]),
                                            Bio = Convert.ToString(dr["ActorBio"])
                                        };

                                        actors.Add(tempActor);
                                    }

                                    movie.Actors = actors;
                                }
                            }
                        }
                        
                        conn.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }

            return movie;
        }
        
        /// <summary>
        /// Data layer function to remove existing actor-movie mapping
        /// </summary>
        /// <param name="movieId"></param>
        public void RemoveActorMovieMapping(int movieId)
        {
            try
            {
                using (MySqlConnection conn = _dbFactory.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "removeactormoviemappings";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("par_movieid", movieId);
                    
                        cmd.Connection = conn;
                    
                        conn.Open();

                        cmd.ExecuteNonQuery();
                        
                        conn.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Data layer function to update movie related data in database.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool UpdateMovie(SaveMovieBundle input)
        {
            bool isUpdated = false;
            
            try
            {
                using (MySqlConnection conn = _dbFactory.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "updatemovie";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        
                        cmd.Parameters.AddWithValue("par_movieid", input.Id);
                        cmd.Parameters.AddWithValue("par_name", input.MovieTitle);
                        cmd.Parameters.AddWithValue("par_releasedate", input.ReleaseDate.Date);
                        cmd.Parameters.AddWithValue("par_plot", input.Plot);
                        cmd.Parameters.AddWithValue("par_poster", input.PosterPath);
                        cmd.Parameters.AddWithValue("par_producerid", input.ProducerId);
                    
                        conn.Open();

                        isUpdated = cmd.ExecuteNonQuery() > 0;
                        
                        conn.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }

            return isUpdated;
        }
    }
}