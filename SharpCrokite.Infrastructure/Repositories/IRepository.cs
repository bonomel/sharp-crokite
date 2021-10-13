using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpCrokite.Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        T Get(int id);
        IEnumerable<T> All();
        T Delete(T entity);
        void DeleteAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void SaveChanges();
    }
}
