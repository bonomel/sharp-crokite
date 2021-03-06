using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace SharpCrokite.Core.PriceRetrievalService.EveMarketerPriceRetrieval
{
    internal class EveMarketerPriceRetrievalService : PriceRetrievalServiceBase
    {
        private const string BaseUrl = "http://api.evemarketer.com/ec/marketstat/json";
        private const int BatchSize = 200;

        public override string ServiceName => "Eve Marketer";

        protected override async Task<IEnumerable<PriceDto>> RetrievePricesFromService(IList<int> allTypeIds)
        {
            IList<IList<int>> batches = CreateBatches(allTypeIds);

            IEnumerable<string> batchedUrls = CreateBatchedUrls(batches);

            IEnumerable<EveMarketerPricesJson> priceJson = await RetrievePricesAsJson(batchedUrls);

            IEnumerable<PriceDto> priceDtos = MapJsonToPriceDto(priceJson);

            return priceDtos;
        }

        private static IEnumerable<PriceDto> MapJsonToPriceDto(IEnumerable<EveMarketerPricesJson> priceJson)
        {
            return priceJson.Select(json => new PriceDto
            {
                TypeId = json.buy.forQuery.types.First(),
                SystemId = json.buy.forQuery.systems.First(),
                BuyMax = json.buy.max,
                BuyMin = json.buy.min,
                BuyPercentile = json.buy.fivePercent,
                SellMax = json.sell.max,
                SellMin = json.sell.min,
                SellPercentile = json.sell.fivePercent
            }).ToList();
        }

        private static async Task<IEnumerable<EveMarketerPricesJson>> RetrievePricesAsJson(IEnumerable<string> batchedUrls)
        {
            List<EveMarketerPricesJson> priceJson = new();
            try
            {
                using HttpClient client = new();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                foreach (string url in batchedUrls)
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        List<EveMarketerPricesJson> batchJsonResult = JsonSerializer.Deserialize<List<EveMarketerPricesJson>>(responseString);

                        if (batchJsonResult != null)
                        {
                            priceJson.AddRange(batchJsonResult);
                        }
                    }
                    else
                    {
                        throw new HttpRequestException(
                            $"Something went wrong while retrieving prices.\nBase URL: {BaseUrl}\nResponse code: {response.StatusCode}\nReason: {response.ReasonPhrase}",
                            null, response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _ = MessageBox.Show(ex.Message, nameof(HttpRequestException), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return priceJson;
        }

        private IEnumerable<string> CreateBatchedUrls(IList<IList<int>> batches)
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

            foreach (int typeId in typeIds)
            {
                _ = stringBuilder.Append($"&typeid={typeId}");
            }

            return stringBuilder.ToString();
        }
    }
}
