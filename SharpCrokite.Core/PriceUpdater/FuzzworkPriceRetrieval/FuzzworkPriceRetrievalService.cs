using System.Collections.Generic;

namespace SharpCrokite.Core.PriceUpdater.FuzzworkPriceRetrieval
{
    public class FuzzworkPriceRetrievalService : IPriceRetrievalService
    {
        private const string BaseUrl = "https://market.fuzzwork.co.uk/aggregates/";
        private readonly Dictionary<int, string> systemsToGetPricesFor = new() // TODO: pull up
        {
            { 30000142, "Jita" }
        };

        public IEnumerable<PriceDto> Retrieve(IList<int> allTypeIds)
        {
            IEnumerable<FuzzworkPricesJson> priceJson = RetrievePricesAsJson();

            IEnumerable<PriceDto> priceDtos = MapJsonToPriceDto(priceJson);

            return priceDtos;
        }

        private IEnumerable<FuzzworkPricesJson> RetrievePricesAsJson()
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<PriceDto> MapJsonToPriceDto(IEnumerable<FuzzworkPricesJson> priceJson)
        {
            throw new System.NotImplementedException();
        }
    }
}