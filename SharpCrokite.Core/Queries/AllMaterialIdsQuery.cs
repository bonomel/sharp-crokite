using System.Collections.Generic;
using System.Linq;

namespace SharpCrokite.DataAccess.Queries
{
    public class AllMaterialIdsQuery
    {
        private readonly SharpCrokiteDbContext dbContext;

        public AllMaterialIdsQuery(SharpCrokiteDbContext dbContext)
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
