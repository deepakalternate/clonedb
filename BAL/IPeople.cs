using System.Collections.Generic;
using CloneDB.Entities;

namespace CloneDB.BAL
{
    public interface IPeople
    {
        bool AddPerson(Person input);
        IEnumerable<Person> GetAllPeople();
    }
}