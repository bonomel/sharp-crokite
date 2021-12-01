using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using SharpCrokite.DataAccess.DatabaseContexts;


namespace SharpCrokite.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly SharpCrokiteDbContext DbContext;

        protected GenericRepository(SharpCrokiteDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual T Add(T entity)
        {
            return DbContext.Add(entity).Entity;
        }

        public virtual IEnumerable<T> All()
        {
            return DbContext.Set<T>().ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().AsQueryable().Where(predicate).ToList();
        }

        public virtual T Get(int id)
        {
            return DbContext.Find<T>(id);
        }

        public virtual void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public virtual T Update(T entity)
        {
            return DbContext.Update(entity).Entity;
        }

        public virtual T Delete(T entity)
        {
            return DbContext.Remove(entity).Entity;
        }

        public virtual void DeleteAll()
        {
            DbContext.Set<T>().RemoveRange(All());
        }
    }
}
