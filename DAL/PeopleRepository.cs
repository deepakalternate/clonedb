using System;
using System.Collections.Generic;
using System.Data;
using CloneDB.Entities;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace CloneDB.DAL
{
    public class PeopleRepository : IPeopleRepository
    {
        
        private readonly IDbFactory _dbFactory;

        public PeopleRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        
        /// <summary>
        /// Data layer function to save data entry for a person in the database.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool AddPerson(Person input)
        {
            bool isInserted = false;

            try
            {
                using (MySqlConnection conn = _dbFactory.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "addperson";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("par_name", input.Name);
                        cmd.Parameters.AddWithValue("par_dob", input.DOB.Date);
                        cmd.Parameters.AddWithValue("par_sex", (int) input.Sex);
                        cmd.Parameters.AddWithValue("par_bio", input.Bio);
                    
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
        /// Data layer function to get all the people (actor and producer) information from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Person> GetAllPeople()
        {
            string dbQuery = @"SELECT Id, Name, DOB, Sex, Bio FROM people;";
            Person temp = null;
            IEnumerable<Person> result = null;

            try
            {
                using (MySqlConnection conn = _dbFactory.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        List<Person> personList = new List<Person>();
                    
                        cmd.CommandText = dbQuery;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;
                    
                        conn.Open();

                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    temp = new Person
                                    {
                                        Id = Convert.ToInt32(dr["Id"]),
                                        Name = Convert.ToString(dr["Name"]),
                                        DOB = Convert.ToDateTime(dr["DOB"]),
                                        Sex = (Sex) Convert.ToInt32(dr["Sex"]),
                                        Bio = Convert.ToString(dr["Bio"])
                                    };

                                    personList.Add(temp);
                                }
                            }
                        }

                        result = personList;
                        
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
            
            return result;
        }
    }
}