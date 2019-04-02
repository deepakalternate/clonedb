using System;
using System.Collections.Generic;
using CloneDB.DAL;
using CloneDB.Entities;

namespace CloneDB.BAL
{
    public class People : IPeople
    {
        private readonly IPeopleRepository _peopleRepository;

        public People(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }
        
        public bool AddPerson(Person input)
        {
            bool isInserted = false;
            try
            {
                isInserted = _peopleRepository.AddPerson(input);
            }
            catch (Exception)
            {
                throw;
            }

            return isInserted;
        }

        public IEnumerable<Person> GetAllPeople()
        {
            IEnumerable<Person> result = null;
            
            try
            {
                result = _peopleRepository.GetAllPeople();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}