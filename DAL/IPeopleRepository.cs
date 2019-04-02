using System.Collections.Generic;
using CloneDB.Entities;

namespace CloneDB.DAL
{
    public interface IPeopleRepository
    {
        bool AddPerson(Person input);
        IEnumerable<Person> GetAllPeople();
    }
}