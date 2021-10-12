using Microsoft.EntityFrameworkCore;
using SharpCrokite.DataAccess;
using SharpCrokite.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharpCrokite.Infrastructure.Repositories
{
    public class MaterialRepository : GenericRepository<Material>
    {
        public MaterialRepository(SharpCrokiteDbContext dbContext) : base(dbContext)
        {
        }

        public override Material Update(Material entity)
        {
            var material = dbContext.Materials
                .Include(m => m.Prices)
                .Single(m => m.MaterialId == entity.MaterialId);

            material.Name = entity.Name;
            material.Description = entity.Description;
            material.Type = material.Type;
            material.Icon = material.Icon;
            material.Prices = material.Prices;

            return base.Update(material);
        }

        public override IEnumerable<Material> Find(Expression<Func<Material, bool>> predicate)
        {
            return dbContext.Materials
                .Include(m => m.Prices)
                .Where(predicate)
                .ToList();
        }

        public override IEnumerable<Material> All()
        {
            return dbContext.Materials
                .Include(m => m.Prices)
                .ToList();
        }
    }
}
