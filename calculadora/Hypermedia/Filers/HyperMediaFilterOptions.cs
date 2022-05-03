using Calculadora.Hypermedia.Abstract;
using System.Collections.Generic;

namespace Calculadora.Hypermedia.Filers
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}