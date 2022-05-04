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
            List<PriceRetrievalServiceOption> priceRetrievalServiceOptions = new();
            foreach (Type type in typeof(PriceRetrievalServiceBase).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(PriceRetrievalServiceBase))))
            {
                priceRetrievalServiceOptions.Add(
                    new PriceRetrievalServiceOption
                    {
                        OptionName = ((PriceRetrievalServiceBase)Activator.CreateInstance(type))?.ServiceName,
                        ServiceType = type
                    }
                );
            }
            
            return priceRetrievalServiceOptions.OrderBy(option => option.OptionName);
        }
    }

    public class PriceRetrievalServiceOption
    {
        [UsedImplicitly] public string OptionName { get; set; }
        public Type ServiceType { get; init; }
    }
}
