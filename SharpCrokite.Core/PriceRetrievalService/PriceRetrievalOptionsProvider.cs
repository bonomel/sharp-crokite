using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SharpCrokite.Core.PriceRetrievalService.EveMarketerPriceRetrieval;
using SharpCrokite.Core.PriceRetrievalService.FuzzworkPriceRetrieval;

namespace SharpCrokite.Core.PriceRetrievalService
{
    public static class PriceRetrievalOptionsProvider
    {
        public static IEnumerable<PriceRetrievalServiceOption> Build()
        {
            return new List<PriceRetrievalServiceOption>
            {
                new()
                {
                    OptionName = "Eve Marketer",
                    ServiceType = typeof(EveMarketerPriceRetrievalService)
                },
                new()
                {
                    OptionName = "Fuzzwork",
                    ServiceType = typeof(FuzzworkPriceRetrievalService)
                }
            };
        }
    }

    public class PriceRetrievalServiceOption
    {
        [UsedImplicitly] public string OptionName { get; set; }
        public Type ServiceType { get; init; }
    }
}
