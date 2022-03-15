using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace SharpCrokite.Core.PriceUpdater
{
    public class EveMarketerPriceRetrievalService : IPriceRetrievalService
    {
        private const string BaseUrl = "http://api.evemarketer.com/ec/marketstat/json";
        private const int BatchSize = 200;
        private readonly Dictionary<int, string> systemsToGetPricesFor = new()
        {
            { 30000142, "Jita" }
        };

        internal IList<PriceDto> Retrieve(IList<int> allTypeIds)
        {
            IList<IList<int>> batches = CreateBatches(allTypeIds);

            IEnumerable<string> batchedUrls = CreateBatchedUrls(batches);

            IEnumerable<EveMarketerPricesJson> priceJson = RetrievePricesAsJson(batchedUrls);

            IList<PriceDto> priceDtos = MapJsonToPriceDto(priceJson);

            return priceDtos;
        }

        private static IList<PriceDto> MapJsonToPriceDto(IEnumerable<EveMarketerPricesJson> priceJson)
        {
            List<PriceDto> priceDtos = new();

            foreach(EveMarketerPricesJson json in priceJson)
            {
                priceDtos.Add(new PriceDto
                {
                    TypeId = json.buy.forQuery.types.First(),
                    SystemId = json.buy.forQuery.systems.First(),
                    BuyMax = json.buy.max,
                    BuyMin = json.buy.min,
                    BuyPercentile = json.buy.fivePercent,
                    SellMax = json.sell.max,
                    SellMin = json.sell.min,
                    SellPercentile = json.sell.fivePercent
                });
            }
            return priceDtos;
        }

        private static IEnumerable<EveMarketerPricesJson> RetrievePricesAsJson(IEnumerable<string> batchedUrls)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            List<EveMarketerPricesJson> priceJson = new();
            foreach(string url in batchedUrls)
            {
                HttpResponseMessage response = client.GetAsync(url).Result;

                if(response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<EveMarketerPricesJson> batchJsonResult = JsonSerializer.Deserialize<List<EveMarketerPricesJson>>(responseString);

                    if (batchJsonResult != null)
                    {
                        priceJson.AddRange(batchJsonResult);
                    }
                }
                else
                {
                    _ = MessageBox.Show($"Something went wrong calling API:\n{url}");
                }
            }
            return priceJson;
        }

        private IEnumerable<string> CreateBatchedUrls(IList<IList<int>> batches)
        {
            List<string> batchedUrls = new();
            
            foreach (KeyValuePair<int, string> system in systemsToGetPricesFor)
            {
                foreach (IList<int> batch in batches)
                {
                    batchedUrls.Add(UrlBuilder(system.Key, batch));
                }
            }
            return batchedUrls;
        }

        private static IList<IList<int>> CreateBatches(ICollection<int> allTypeIds)
        {
            List<IList<int>> batches = new();

            for (int i = 0; i < allTypeIds.Count; i += BatchSize)
            {
                batches.Add(allTypeIds.ToList().GetRange(i, Math.Min(BatchSize, allTypeIds.Count - i)));
            }
            return batches;
        }

        private static string UrlBuilder(int systemKey, IEnumerable<int> typeIds)
        {
            StringBuilder stringBuilder = new();

            _ = stringBuilder.Append($"{BaseUrl}?usesystem={systemKey}");

            foreach(int typeId in typeIds)
            {
                _ = stringBuilder.Append($"&typeid={typeId}");
            }

            return stringBuilder.ToString();
        }
    }

    public interface IPriceRetrievalService
    {
    }
}
