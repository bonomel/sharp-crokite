using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpCrokite.Core.PriceUpdater
{
    public interface IPriceRetrievalService
    {
        public Task<IEnumerable<PriceDto>> Retrieve(IList<int> allTypeIds);
    }
}