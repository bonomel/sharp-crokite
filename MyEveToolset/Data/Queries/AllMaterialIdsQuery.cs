using System.Collections.Generic;
using System.Linq;

namespace MyEveToolset.Data.Queries
{
    public class AllMaterialIdsQuery
    {
        private readonly MyEveToolDbContext dbContext;

        public AllMaterialIdsQuery(MyEveToolDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<int> Execute()
        {
            IList<int> materialIdList = dbContext.Materials.Select(m => m.MaterialId).ToList();

            return materialIdList;
        }
    }
}
