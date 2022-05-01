using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpCrokite.Core.PriceRetrievalService
{
    public abstract class PriceRetrievalServiceBase : IPriceRetrievalService
    {
        public abstract string OptionName { get; }

        public Task<IEnumerable<PriceDto>> Retrieve(IList<int> allTypeIds)
        {
            // add exception handling
            return RetrievePricesFromService(allTypeIds);
        }

        protected abstract Task<IEnumerable<PriceDto>> RetrievePricesFromService(IList<int> allTypeIds);
    }
}
