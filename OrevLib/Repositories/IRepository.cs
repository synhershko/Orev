using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Orev.Repositories
{
    public interface IRepository<T>
        where T : class, Orev.Domain.IOrevEntity
    {
        void Commit();
        void Rollback();

        void Add(T item);
        void Update(T item);
        void Delete(T item);

        T SingleOrDefault(Expression<Func<T, bool>> query);
        IEnumerable<T> All(Expression<Func<T, bool>> query);
        int Count(Expression<Func<T, bool>> query);
        IEnumerable<T> Distinct(IEqualityComparer<T> comparer);
    }
}
