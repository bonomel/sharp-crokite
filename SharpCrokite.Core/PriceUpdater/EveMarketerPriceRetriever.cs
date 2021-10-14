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
    public class EveMarketerPriceRetriever
    {
        private const string BaseUrl = "http://api.evemarketer.com/ec/marketstat/json";
        private const int BatchSize = 200;
        private readonly Dictionary<int, string> SystemsToGetPricesFor = new()
        {
            { 30000142, "Jita" }
        };

        internal IList<PriceDto> Retrieve(IList<int> allTypeIds)
        {
            IList<IList<int>> batches = CreateBatches(allTypeIds);

            IList<string> batchedUrls = CreateBatchedUrls(batches);

            IEnumerable<EveMarketerPricesJson> priceJson = RetrievePricesAsJson(batchedUrls);

            IList<PriceDto> priceDtos = TransformToPriceDtos(priceJson);

            return priceDtos;
        }

        private IList<PriceDto> TransformToPriceDtos(IEnumerable<EveMarketerPricesJson> priceJson)
        {
            List<PriceDto> priceDtos = new();

            foreach(EveMarketerPricesJson json in priceJson)
            {
                priceDtos.Add(new PriceDto()
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

        private IEnumerable<EveMarketerPricesJson> RetrievePricesAsJson(IList<string> batchedUrls)
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

                    priceJson.AddRange(batchJsonResult);
                }
                else
                {
                    MessageBox.Show($"Something went wrong calling API:\n{url}");
                }
            }
            return priceJson;
        }

        private IList<string> CreateBatchedUrls(IList<IList<int>> batches)
        {
            List<string> batchedUrls = new();
            
            foreach (KeyValuePair<int, string> system in SystemsToGetPricesFor)
            {
                foreach (IList<int> batch in batches)
                {
                    batchedUrls.Add(UrlBuilder(system.Key, batch));
                }
            }
            return batchedUrls;
        }

        private static IList<IList<int>> CreateBatches(IList<int> allTypeIds)
        {
            List<IList<int>> batches = new();

            for (int i = 0; i < allTypeIds.Count; i += BatchSize)
            {
                batches.Add(allTypeIds.ToList().GetRange(i, Math.Min(BatchSize, allTypeIds.Count - i)));
            }
            return batches;
        }

        private string UrlBuilder(int systemKey, IEnumerable<int> typeIds)
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
}
// http://api.evemarketer.com/ec/marketstat?usesystem=30000144&typeid=18&typeid=34&typeid=35&typeid=36&typeid=37&typeid=38&typeid=39&typeid=40&typeid=1224&typeid=1228&typeid=1230&typeid=17455&typeid=17456&typeid=17459&typeid=17460&typeid=17463&typeid=17464&typeid=17470&typeid=17471&typeid=11399&typeid=28432&typeid=28429&typeid=28424&typeid=28422&typeid=28416&typeid=28410&typeid=28406&typeid=28403&typeid=28401&typeid=28397&typeid=28394&typeid=28420&typeid=28391&typeid=28388&typeid=28367&typeid=45492&typeid=45493&typeid=45491&typeid=45490&typeid=16633&typeid=16636&typeid=16635&typeid=16634