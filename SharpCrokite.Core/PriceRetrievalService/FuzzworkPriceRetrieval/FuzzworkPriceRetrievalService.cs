using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using JetBrains.Annotations;
using SharpCrokite.Core.PriceUpdater.FuzzworkPriceRetrieval;

namespace SharpCrokite.Core.PriceRetrievalService.FuzzworkPriceRetrieval
{
    [UsedImplicitly]
    internal class FuzzworkPriceRetrievalService : PriceRetrievalServiceBase
    {
        private const string BaseUrl = "https://market.fuzzwork.co.uk/aggregates/";

        public override string ServiceName => "Fuzzwork";

        protected override async Task<IEnumerable<PriceDto>> RetrievePricesFromService(IList<int> allTypeIds)
        {
            List<PriceDto> priceDtos = new();

            foreach (int systemId in SystemsToGetPricesFor.Keys)
            {
                Dictionary<string, FuzzworkPricesJson> pricesPerItemForSystem = await RetrievePricesAsJson(BuildUrl(allTypeIds, systemId));

                priceDtos.AddRange(MapJsonToPriceDto(pricesPerItemForSystem, systemId));
            }

            return priceDtos;
        }

        private static async Task<Dictionary<string, FuzzworkPricesJson>> RetrievePricesAsJson(string url)
        {
            Dictionary<string, FuzzworkPricesJson> priceJson = new();

            try
            {
                using HttpClient client = new();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    priceJson = JsonSerializer.Deserialize<Dictionary<string, FuzzworkPricesJson>>(responseString);
                }
                else
                {
                    throw new HttpRequestException(
                        $"Something went wrong while retrieving prices.\nBase URL: {BaseUrl}\nResponse code: {response.StatusCode}\nReason: {response.ReasonPhrase}",
                        null, response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                _ = MessageBox.Show(ex.Message, nameof(HttpRequestException), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return priceJson;
        }

        private static string BuildUrl(IEnumerable<int> allTypeIds, int systemId)
        {
            StringBuilder stringBuilder = new();

            _ = stringBuilder.Append($"{BaseUrl}?region={systemId}&types=");

            foreach (int typeId in allTypeIds)
            {
                _ = stringBuilder.Append($"{typeId},");
            }

            return stringBuilder.ToString();
        }

        private static IEnumerable<PriceDto> MapJsonToPriceDto(Dictionary<string, FuzzworkPricesJson> pricesAsJson, int systemKey)
        {
            List<PriceDto> priceDtos = new();

            foreach ((string key, FuzzworkPricesJson value) in pricesAsJson)
            {
                priceDtos.Add(new PriceDto
                {
                    TypeId = Convert.ToInt32(key),
                    SystemId = systemKey,
                    BuyMax = Convert.ToDecimal(value.buy.max),
                    BuyMin = Convert.ToDecimal(value.buy.min),
                    BuyPercentile = Convert.ToDecimal(value.buy.percentile),
                    SellMax = Convert.ToDecimal(value.sell.max),
                    SellMin = Convert.ToDecimal(value.sell.min),
                    SellPercentile = Convert.ToDecimal(value.sell.percentile)
                });
            }
            return priceDtos;
        }
    }
}