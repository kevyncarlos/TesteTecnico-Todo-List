using System.Linq.Expressions;
using TesteTodoList.API.Models.Entities;

namespace TesteTodoList.API.Data.Repositories.Interfaces;

public interface IRepositoryBase<T> where T : EntityBase
{
    Task<T?> GetByIdAsync(int id);
    IQueryable<T> GetBy(Expression<Func<T, bool>>? filter = null);
    Task<int> GetTotalByAsync(Expression<Func<T, bool>>? filter = null);
    Task<IQueryable<T>> GetPaginateByAsync(Expression<Func<T, bool>>? filter = null, int pageIndex = 0, int pageSize = 10);
    Task<T> AddOrUpdateAsync(T entity);
    Task<List<T>> AddOrUpdateRangeAsync(List<T> entities);
    Task<bool> DeleteOrRestoreAsync(int id);
    Task<bool> DeletePermanentAsync(int id);
    Task<int> SaveChangesAsync();

}
