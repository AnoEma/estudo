using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Calculadora.Hypermedia.Abstract
{
    public interface IResponseEnricher
    {
        Task Enrich(ResultExecutingContext context);
        bool CanEnrich(ResultExecutingContext context);
    }
}