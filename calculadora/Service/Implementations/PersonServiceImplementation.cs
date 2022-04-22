using Calculadora.Repository;
using Estudo.Model;
using System.Collections.Generic;

namespace Estudo.Service.Implementations
{
    public class PersonServiceImplementation : IPersonService
    {
        private readonly IPersonRepository _repository;
        public PersonServiceImplementation(IPersonRepository repository)
        {
            _repository = repository;
        }

        public Person Create(Person person)
        {
            return _repository.Create(person);
        }

        public void Delete(long id)
        {
           _repository.Delete(id);
        }

        public List<Person> FindAll()
        {
            return _repository.FindAll();
        }

        public Person FindById(long id)
        {
            return _repository.FindById(id);
        }

        public Person Update(Person person)
        {
            return _repository.Update(person);
        }
    }
}