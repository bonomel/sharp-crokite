using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpCrokite.Core.PriceRetrievalService
{
    public interface IPriceRetrievalService
    {
        public Task<IEnumerable<PriceDto>> Retrieve(IList<int> allTypeIds);
    }
}