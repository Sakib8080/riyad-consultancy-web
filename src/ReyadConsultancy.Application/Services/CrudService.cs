using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Common;

namespace ReyadConsultancy.Application.Services;

public class CrudService<T> : ICrudService<T> where T : BaseEntity
{
    private readonly IApplicationDbContext _db;

    public CrudService(IApplicationDbContext db) => _db = db;

    public async Task<List<T>> ListAsync(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _db.Set<T>().AsNoTracking();
        foreach (var include in includes) query = query.Include(include);

        query = typeof(ISortable).IsAssignableFrom(typeof(T))
            ? query.OrderBy(e => EF.Property<int>(e, nameof(ISortable.SortOrder))).ThenBy(e => e.Id)
            : query.OrderByDescending(e => e.Id);

        return await query.ToListAsync();
    }

    public Task<T?> GetAsync(int id) => _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

    public async Task<T> CreateAsync(T entity)
    {
        entity.Id = 0;
        _db.Set<T>().Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        var existing = await _db.Set<T>().FirstOrDefaultAsync(e => e.Id == entity.Id);
        if (existing == null) throw new KeyNotFoundException($"{typeof(T).Name} {entity.Id} was not found.");

        entity.CreatedAt = existing.CreatedAt;          // never overwrite the original creation date
        _db.Entry(existing).CurrentValues.SetValues(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _db.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        if (existing == null) return;
        _db.Set<T>().Remove(existing);
        await _db.SaveChangesAsync();
    }
}
