using Estudo.Model;
using System.Collections.Generic;
using System.Threading;

namespace Estudo.Service.Implementations
{
    public class PersonServiceImplementation : IPersonService
    {
        private List<Person> persons;
        private volatile int count;

        public Person Create(Person person)
        {
            return person;
        }

        public void Delete(long id)
        {
            throw new System.NotImplementedException();
        }

        public List<Person> FindAll()
        {
            persons = new List<Person>();
            for (int i = 0; i < 8; i++)
            {
                Person person = MockPerson(i);
                persons.Add(person);
            }
            return persons;
        }

        public Person FindById(long id)
        {
            return new Person
            {
                Id = IncrementAndGet(),
                FirstName = "Arnoboys",
                LastName = "Emangi",
                Address = "Rua Cores Vivas - São Paulo - Brasil",
                Gender = "Male"
            };
        }

        public Person Update(Person person)
        {
            return person;
        }

        private Person MockPerson(int i)
        {
            return new Person
            {
                Id = IncrementAndGet(),
                FirstName = "Arnoboys " + i,
                LastName = "Emangi " + i,
                Address = "Rua Cores Vivas - São Paulo - Brasil " + i,
                Gender = "Male" 
            };
        }

        private long IncrementAndGet()
        {
            return Interlocked.Increment(ref count);
        }
    }
}
