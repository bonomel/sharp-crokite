using System;
using System.Collections.Generic;
using SharpCrokite.Core.PriceUpdater.EveMarketerPriceRetrieval;
using SharpCrokite.Core.PriceUpdater.FuzzworkPriceRetrieval;

namespace SharpCrokite.Core.PriceUpdater
{
    public static class PriceRetrievalOptionsBuilder
    {
        public static IEnumerable<PriceRetrievalServiceOption> Build()
        {
            return new List<PriceRetrievalServiceOption>()
            {
                new()
                {
                    HumanReadableOptionValue = "Eve Marketer",
                    ServiceType = typeof(EveMarketerPriceRetrievalService)
                },
                new()
                {
                    HumanReadableOptionValue = "Fuzzwork",
                    ServiceType = typeof(FuzzworkPriceRetrievalService)
                }
            };
        }
    }

    public class PriceRetrievalServiceOption
    {
        public string HumanReadableOptionValue { get; set; }
        public Type ServiceType { get; set; }
    }
}
