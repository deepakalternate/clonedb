using System;
using System.Collections.Generic;
using CloneDB.BAL;
using CloneDB.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CloneDB.Controllers
{
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPeople _people;

        public PeopleController(IPeople people)
        {
            _people = people;
        }
        
        /// <summary>
        /// API end point to add an entry of person (Actor or Producer)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/people/save")]
        public ActionResult<bool> SavePerson([FromBody]Person input)
        {
            bool result = false;

            try
            {
                result = _people.AddPerson(input);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
        
        /// <summary>
        /// API end point to get the list of all the people related to movies (Actors and Producers)
        /// Actors can also become Producers and vice-versa.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/people")]
        public ActionResult<IEnumerable<People>> GetAllPeople()
        {
            IEnumerable<Person> result = null;
            try
            {
                result = _people.GetAllPeople();

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
    }
}