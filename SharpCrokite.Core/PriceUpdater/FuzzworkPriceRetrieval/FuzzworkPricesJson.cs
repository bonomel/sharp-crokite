// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

#pragma warning disable IDE1006 // Naming Styles
namespace SharpCrokite.Core.PriceUpdater.FuzzworkPriceRetrieval
{
    public class FuzzworkPricesJson
    {
        public Buy buy { get; set; }

        public Sell sell { get; set; }

        public class Buy
        {
            //public string weightedAverage { get; set; }
            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public decimal max { get; set; }
            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public decimal min { get; set; }
            //public string stddev { get; set; }
            //public string median { get; set; }
            //public string volume { get; set; }
            //public string orderCount { get; set; }
            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public decimal percentile { get; set; }
        }

        public class Sell
        {
            //public string weightedAverage { get; set; }
            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public decimal max { get; set; }
            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public decimal min { get; set; }
            //public string stddev { get; set; }
            //public string median { get; set; }
            //public string volume { get; set; }
            //public string orderCount { get; set; }
            [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
            public decimal percentile { get; set; }
        }
    }
}
