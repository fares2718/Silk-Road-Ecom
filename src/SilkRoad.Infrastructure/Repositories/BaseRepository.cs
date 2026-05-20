using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

internal class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    /// <summary>
    ///   Retrieves a list of entities of type T, including related entities specified by the includeProperties parameters. The method constructs a query that applies the specified includes to eagerly load related data and returns the results as a list.
    /// </summary>
    /// <param name="includeProperties"></param>
    /// <returns>IReadOnlyList<T></returns>
    public async Task<IReadOnlyList<TDTO>> GetAllAndIncludeAsync<TKey, TDTO>(Expression<Func<T, TDTO>> selector,
    Expression<Func<T, TKey>>? orderBy = null,
    bool isDescending = false
    ) where TDTO : class
    {
        var query = _context.Set<T>().AsNoTracking().AsQueryable();

        if (orderBy != null)
        {
            query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        return await query
        .Select(selector)
        .ToListAsync();
    }

    /// <summary>
    /// Performs a join between the primary entity T and a target entity TTarget based on specified key selectors and a result selector, returning a list of TResult.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="outerKeySelector"></param>
    /// <param name="innerKeySelector"></param>
    /// <param name="resultSelector"></param>
    /// <returns>IReadOnlyList<TResult></returns>
    public async Task<IReadOnlyList<TResult>> GetAllAndJoinAsync<TTarget, TKey, TResult>(
    Expression<Func<T, TKey>> outerKeySelector,
    Expression<Func<TTarget, TKey>> innerKeySelector,
    Expression<Func<T, TTarget, TResult>> resultSelector) where TTarget : class
    {
        return await _context.Set<T>().AsNoTracking().AsQueryable()
    .Join(
        _context.Set<TTarget>().AsNoTracking(),
        outerKeySelector,
        innerKeySelector,
        resultSelector
    )
    .ToListAsync();
    }

    public async Task<IReadOnlyList<TDTO>> GetAllAsync<TDTO>(Expression<Func<T, TDTO>> selector) where TDTO : class
    {
        return await _context.Set<T>()
            .AsNoTracking()
            .Select(selector)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves an entity of type T by its primary key (id) and includes related entities specified by the includeProperties parameters. The method dynamically determines the primary key property name using EF Core metadata, constructs a query that filters by the primary key, and applies the specified includes to eagerly load related data. It returns a single entity or null if not found.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeProperties"></param>
    /// <returns>T object</returns>
    public async Task<TDTO?> GetByIdAndIncludeAsync<TDTO>(int id,Expression<Func<T, TDTO>> selector) where TDTO : class
    {
        string keyName = GetPrimaryKeyName();
        var query = _context.Set<T>().AsNoTracking().AsQueryable().Where(e => EF.Property<int>(e, keyName) == id);

        return await query.Select(selector).SingleOrDefaultAsync();
    }

    /// <summary>
    /// Performs a join between the primary entity T and a target entity TTarget based on specified key selectors and a result selector, returning an object of TResult.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="outerKeySelector"></param>
    /// <param name="innerKeySelector"></param>
    /// <param name="resultSelector"></param>
    /// <returns>TResult object</returns>
    public async Task<TResult?> GetByIdAndJoinAsync<TTarget, TKey, TResult>(int id,
    Expression<Func<T, TKey>> outerKeySelector,
    Expression<Func<TTarget, TKey>> innerKeySelector,
    Expression<Func<T, TTarget, TResult>> resultSelector) where TTarget : class
    {
        // Find the primary key property name dynamically via EF Core Metadata
        var keyName = GetPrimaryKeyName();

        return await _context.Set<T>().AsNoTracking().AsQueryable().Where(e => EF.Property<int>(e, keyName) == id)
        .Join(
            _context.Set<TTarget>().AsNoTracking(),
            outerKeySelector,
            innerKeySelector,
            resultSelector
        ).SingleOrDefaultAsync();
    }
    public async Task<TDTO?> GetByIdAsync<TDTO>(int id, Expression<Func<T, TDTO>> selector) where TDTO : class
    {
        string keyName = GetPrimaryKeyName();
        return await _context.Set<T>().Where(e => EF.Property<int>(e, keyName) == id).Select(selector).SingleOrDefaultAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        string keyName = GetPrimaryKeyName();
        T? existingEntity = await _context.Set<T>()
        .FirstOrDefaultAsync(e => EF.Property<int>(e, keyName) 
        == EF.Property<int>(entity, keyName));
        if(existingEntity == null)
        {
            throw new InvalidOperationException($"Entity of type {typeof(T).Name} with the specified ID does not exist.");
        }
        var props = typeof(T).GetProperties().Where(p => p.Name != keyName);
        foreach(var prop in props)
        {
            var newValue = prop.GetValue(entity);
            if(newValue is null || (newValue is string str && string.IsNullOrEmpty(str)))
                continue;
            else
                prop.SetValue(existingEntity, newValue);
        }
        int rowsAffected = await _context.SaveChangesAsync();
        if(rowsAffected == 0)
            throw new InvalidOperationException($"Failed to update entity of type {typeof(T).Name} with the specified ID.");
        if(rowsAffected > 1)
            throw new InvalidOperationException($"Unexpected number of rows affected ({rowsAffected}) when updating entity of type {typeof(T).Name} with the specified ID.");
    }


    private string GetPrimaryKeyName()
    {
        var key = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey();
        if (key == null)
        {
            throw new InvalidOperationException($"No primary key defined for entity {typeof(T).Name}");
        }
        return key.Properties.Select(p => p.Name).FirstOrDefault() ?? throw new InvalidOperationException($"Primary key properties not found for entity {typeof(T).Name}");
    }
}
