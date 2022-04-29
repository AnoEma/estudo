using Calculadora.Data.Converter.Implementation;
using Calculadora.Data.VO;
using Calculadora.Repository;
using System.Collections.Generic;

namespace Estudo.Service.Implementations
{
    public class PersonServiceImplementation : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly PersonConverter  _converter;
        public PersonServiceImplementation
        (
            IPersonRepository repository
        )
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public PersonVO Create(PersonVO person)
        {
            var personVO = _converter.Parse(person);
            personVO = _repository.Create(personVO);
            return _converter.Parse(personVO);
        }

        public void Delete(long id)
        {
           _repository.Delete(id);
        }

        public List<PersonVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public PersonVO FindById(long id)
        {
            return _converter.Parse(_repository.FindById(id));
        }

        public PersonVO Update(PersonVO person)
        {
            var personVO = _converter.Parse(person);
            personVO = _repository.Update(personVO);
            return _converter.Parse(personVO);
        }
    }
}