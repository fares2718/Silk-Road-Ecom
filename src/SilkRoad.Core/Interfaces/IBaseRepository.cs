using System.Linq.Expressions;

namespace SilkRoad.Core;

public interface IBaseRepository<T>where T : class
{
    Task AddAsync(T entity);
    Task DeleteAsync(int id);
    Task<IReadOnlyList<TDTO>> GetAllAsync<TDTO>(Expression<Func<T, TDTO>> selector) where TDTO : class;
    Task<IReadOnlyList<T>> GetAllAndIncludeAsync(params Expression<Func<T, object>>[] includeProperties);
    
    Task<IReadOnlyList<TResult>> GetAllAndJoinAsync<TTarget, TKey, TResult>(
    Expression<Func<T, TKey>> outerKeySelector,
    Expression<Func<TTarget, TKey>> innerKeySelector,
    Expression<Func<T, TTarget, TResult>> resultSelector) where TTarget : class;

    Task<TDTO?> GetByIdAsync<TDTO>(int id, Expression<Func<T, TDTO>> selector) where TDTO : class;
    Task<T?> GetByIdAndIncludeAsync(int id, params Expression<Func<T, object>>[] includeProperties);
    Task<TResult?> GetByIdAndJoinAsync<TTarget, TKey, TResult>(int id,
    Expression<Func<T, TKey>> outerKeySelector,
    Expression<Func<TTarget, TKey>> innerKeySelector,
    Expression<Func<T, TTarget, TResult>> resultSelector) where TTarget : class;
    Task UpdateAsync(T entity);
}
