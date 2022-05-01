using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpCrokite.Core.PriceRetrievalService
{
    public abstract class PriceRetrievalServiceBase : IPriceRetrievalService
    {
        // to be made configurable
        protected readonly Dictionary<int, string> SystemsToGetPricesFor = new()
        {
            { 30000142, "Jita" }
        };

        public abstract string ServiceName { get; }

        public Task<IEnumerable<PriceDto>> Retrieve(IList<int> allTypeIds)
        {
            // add exception handling
            return RetrievePricesFromService(allTypeIds);
        }

        protected abstract Task<IEnumerable<PriceDto>> RetrievePricesFromService(IList<int> allTypeIds);
    }
}
