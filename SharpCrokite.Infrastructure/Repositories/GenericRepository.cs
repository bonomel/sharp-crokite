using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace SharpCrokite.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext dbContext;

        protected GenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual T Add(T entity)
        {
            return dbContext.Add(entity).Entity;
        }

        public virtual IEnumerable<T> All()
        {
            return dbContext.Set<T>().ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().AsQueryable().Where(predicate).ToList();
        }

        public virtual T Get(int id)
        {
            return dbContext.Find<T>(id);
        }

        public virtual void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public virtual T Update(T entity)
        {
            return dbContext.Update(entity).Entity;
        }

        public virtual T Delete(T entity)
        {
            return dbContext.Remove(entity).Entity;
        }

        public virtual void DeleteAll()
        {
            dbContext.Set<T>().RemoveRange(All());
        }
    }
}
