using System.Collections.Generic;
using System.Linq;

namespace MyEveToolset.Data.Queries
{
    public class AllHarvestableIdsQuery
    {
        private readonly SharpCrokiteDbContext dbContext;

        public AllHarvestableIdsQuery(SharpCrokiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<int> Execute()
        {
            IList<int> harvestableIdList = dbContext.Harvestables.Select(h => h.HarvestableId).ToList();

            return harvestableIdList;
        }
    }
}
