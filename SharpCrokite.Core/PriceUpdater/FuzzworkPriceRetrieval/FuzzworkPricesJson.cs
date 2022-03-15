// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable IDE1006 // Naming Styles
namespace SharpCrokite.Core.PriceUpdater.FuzzworkPriceRetrieval
{
    public class FuzzworkPricesJson
    {
        public class Buy
        {
            public string weightedAverage { get; set; }
            public string max { get; set; }
            public string min { get; set; }
            public string stddev { get; set; }
            public string median { get; set; }
            public string volume { get; set; }
            public string orderCount { get; set; }
            public string percentile { get; set; }
        }

        public class Sell
        {
            public string weightedAverage { get; set; }
            public string max { get; set; }
            public string min { get; set; }
            public string stddev { get; set; }
            public string median { get; set; }
            public string volume { get; set; }
            public string orderCount { get; set; }
            public string percentile { get; set; }
        }
    }

    
}
