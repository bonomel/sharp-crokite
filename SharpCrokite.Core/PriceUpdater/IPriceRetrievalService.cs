using System.Collections.Generic;

namespace SharpCrokite.Core.PriceUpdater
{
    public interface IPriceRetrievalService
    {
        public IEnumerable<PriceDto> Retrieve(IList<int> allTypeIds);
    }
}