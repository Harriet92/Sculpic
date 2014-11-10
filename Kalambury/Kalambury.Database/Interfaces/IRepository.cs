using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kalambury.Mongo.Interfaces
{
    public interface IRepository<T>
    {
        T Insert(T item);
        bool DeleteAll();
        bool Delete(T item);
        T Save(T item);
        List<T> GetByQuery(Expression<Func<T, bool>> predicate);
        T GetItemByQuery(Expression<Func<T, bool>> predicate);
        List<T> GetAll();
        T AtomicIncrement(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> updatedProperty, long value);
        T AtomicUpdate<TMember>(Expression<Func<T, bool>> predicate, Expression<Func<T, TMember>> updatedProperty, TMember newValue);
        long CountAll();
    }
}
