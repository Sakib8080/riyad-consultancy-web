using System.Linq.Expressions;
using ReyadConsultancy.Domain.Common;

namespace ReyadConsultancy.Application.Common.Interfaces;

public interface ICrudService<T> where T : BaseEntity
{
    Task<List<T>> ListAsync(params Expression<Func<T, object>>[] includes);
    Task<T?> GetAsync(int id);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
