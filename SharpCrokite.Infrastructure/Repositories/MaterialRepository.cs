using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using SharpCrokite.DataAccess.DatabaseContexts;
using SharpCrokite.DataAccess.Models;

namespace SharpCrokite.Infrastructure.Repositories
{
    public class MaterialRepository : GenericRepository<Material>
    {
        public MaterialRepository(SharpCrokiteDbContext dbContext) : base(dbContext)
        {
        }

        public override Material Update(Material entity)
        {
            var material = DbContext.Materials
                .Include(m => m.Prices)
                .Single(m => m.MaterialId == entity.MaterialId);

            material.Name = entity.Name;
            material.Description = entity.Description;
            material.Type = material.Type;
            material.Quality = entity.Quality;
            material.Icon = material.Icon;
            material.Prices = material.Prices;

            return base.Update(material);
        }

        public override IEnumerable<Material> Find(Expression<Func<Material, bool>> predicate)
        {
            return DbContext.Materials
                .Include(m => m.Prices)
                .Where(predicate)
                .ToList();
        }

        public override IEnumerable<Material> All()
        {
            return DbContext.Materials
                .Include(m => m.Prices)
                .ToList();
        }
    }
}
