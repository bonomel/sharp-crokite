using MyEveToolset.Data;
using MyEveToolset.Data.Queries;
using System.Collections.Generic;
using System.Linq;

namespace MyEveToolset.PriceUpdater
{
    public class PriceUpdateController
    {
        private readonly SharpCrokiteDbContext dbContext;
        private readonly EveMarketerPriceRetriever priceRetriever;
        
        public PriceUpdateController(SharpCrokiteDbContext dbContext, EveMarketerPriceRetriever priceRetriever)
        {
            this.dbContext = dbContext;
            this.priceRetriever = priceRetriever;
        }

        internal void UpdatePrices()
        {
            IList<int> listOfHarvestableIds = new AllHarvestableIdsQuery(dbContext).Execute();
            IList<int> listOfMaterialIds = new AllMaterialIdsQuery(dbContext).Execute();

            IList<int> allTypeIds = listOfHarvestableIds.Concat(listOfMaterialIds).ToList();

            IList<PriceDto> prices = priceRetriever.Retrieve(allTypeIds);

            PriceUpdater priceUpdater = new(dbContext);

            priceUpdater.Update(prices);
        }

        internal void DeleteAllPrices()
        {
            dbContext.Prices.RemoveRange(dbContext.Prices);
            dbContext.SaveChanges();
        }
    }
}
