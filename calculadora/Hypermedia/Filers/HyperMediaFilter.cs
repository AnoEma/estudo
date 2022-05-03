using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Calculadora.Hypermedia.Filers
{
    public class HyperMediaFilter : ResultFilterAttribute
    {
        private readonly HyperMediaFilterOptions _options;

        public HyperMediaFilter(HyperMediaFilterOptions options)
        {
            _options = options;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            TryEnrichResult(context);
            base.OnResultExecuting(context);
        }

        private void TryEnrichResult(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult ob)
            {
                var enricher = _options.ContentResponseEnricherList.FirstOrDefault(x => x.CanEnrich(context));
                if (enricher != null)
                {
                    Task.FromResult(enricher.Enrich(context));
                }
            }
        }
    }
}