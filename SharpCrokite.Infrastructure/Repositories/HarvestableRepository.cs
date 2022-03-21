using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using SharpCrokite.DataAccess.DatabaseContexts;
using SharpCrokite.DataAccess.Models;

namespace SharpCrokite.Infrastructure.Repositories
{
    public class HarvestableRepository : GenericRepository<Harvestable>
    {
        private readonly SharpCrokiteDbContext dbContext;

        public HarvestableRepository(SharpCrokiteDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public override Harvestable Update(Harvestable entity)
        {
            var harvestable = dbContext.Harvestables
                .Include(h => h.Prices)
                .Include(h => h.MaterialContents)
                .Single(h => h.HarvestableId == entity.HarvestableId);

            harvestable.Name = entity.Name;
            harvestable.Description = entity.Description;
            harvestable.Volume = entity.Volume;
            harvestable.Type = entity.Type;
            harvestable.Icon = entity.Icon;
            harvestable.IsCompressedVariantOfTypeId = entity.IsCompressedVariantOfTypeId;
            harvestable.CompressedVariantTypeId = entity.CompressedVariantTypeId;
            harvestable.MaterialContents = entity.MaterialContents;
            harvestable.Prices = entity.Prices;

            return base.Update(harvestable);
        }

        public override IEnumerable<Harvestable> Find(Expression<Func<Harvestable, bool>> predicate)
        {
            return dbContext.Harvestables
                .Include(h => h.Prices)
                .Include(h => h.MaterialContents)
                .ThenInclude(mc => mc.Material)
                .Where(predicate)
                .ToList();
        }

        public override IEnumerable<Harvestable> All()
        {
            return dbContext.Harvestables
                .Include(h => h.Prices)
                .Include(h => h.MaterialContents)
                .ThenInclude(mc => mc.Material)
                .ToList();
        }

        public override Harvestable Get(int id)
        {
            return dbContext.Harvestables.Where(harvestable => harvestable.HarvestableId == id).Include(h => h.Prices)
                .SingleOrDefault();
        }
    }
}
