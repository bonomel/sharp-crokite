using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace SharpCrokite.Core.PriceRetrievalService
{
    public static class PriceRetrievalOptionsProvider
    {
        public static IEnumerable<PriceRetrievalServiceOption> Build()
        {
            return typeof(PriceRetrievalServiceBase).Assembly.GetTypes().Where(type =>
                type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(PriceRetrievalServiceBase))).Select(type =>
                    new PriceRetrievalServiceOption
                    {
                        OptionName = (Activator.CreateInstance(type) as PriceRetrievalServiceBase)?.ServiceName,
                        ServiceType = type
                    }).ToList().OrderBy(option => option.OptionName);
        }
    }

    public class PriceRetrievalServiceOption
    {
        [UsedImplicitly] public string OptionName { get; set; }
        public Type ServiceType { get; init; }
    }
}
