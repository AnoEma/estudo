﻿using Calculadora.Data.VO;
using System.Collections.Generic;

namespace Estudo.Service
{
    public interface IPersonService
    {
        PersonVO Create(PersonVO person);
        PersonVO FindById(long id);
        List<PersonVO> FindAll();
        PersonVO Update(PersonVO person);
        void Delete(long id);
    }
}