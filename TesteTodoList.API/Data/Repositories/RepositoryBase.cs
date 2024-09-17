using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using TesteTodoList.API.Data;
using TesteTodoList.API.Data.Repositories.Interfaces;
using TesteTodoList.API.Models.Entities;

namespace TesteTodoList.API.Data.Repositories;

public class RepositoryBase<T>(TodoDbContext _context) : IRepositoryBase<T> where T : EntityBase
{
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context
            .Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<T> GetBy(Expression<Func<T, bool>>? filter = null)
    {
        var query = _context.Set<T>().AsNoTracking();

        if (filter != null)
            query = query.Where(filter);

        return query;
    }

    public async Task<int> GetTotalByAsync(Expression<Func<T, bool>>? filter = null)
    {
        var query = _context.Set<T>().AsNoTracking();

        if (filter != null)
            query = query.Where(filter);

        return await query.CountAsync();
    }

    public async Task<IQueryable<T>> GetPaginateByAsync(Expression<Func<T, bool>>? filter = null, int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Set<T>().AsNoTracking();

        if (filter != null)
            query = query.Where(filter);

        query = query
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return await Task.FromResult(query);
    }

    public async Task<T> AddOrUpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            entity.UpdatedDate = DateTime.Now;

            if (entity.Id == 0)
            {
                entity.IsActive = true;
                entity.CreatedDate = DateTime.Now;
                await _context.Set<T>().AddAsync(entity);
            }
            else
                _context.Set<T>().Update(entity);

            return entity;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<T>> AddOrUpdateRangeAsync(List<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        try
        {
            if (entities.Exists(x => x.Id != 0))
            {
                var entitiesUpdate = entities.Where(x => x.Id != 0).ToList();

                entitiesUpdate.ForEach(x => x.UpdatedDate = DateTime.Now);

                _context.Set<T>().UpdateRange(entitiesUpdate);
            }


            if (entities.Exists(x => x.Id == 0))
            {
                var entitiesAdd = entities.Where(x => x.Id == 0).ToList();

                entitiesAdd.ForEach(x =>
                {
                    x.IsActive = true;
                    x.CreatedDate = DateTime.Now;
                });

                await _context.Set<T>().AddRangeAsync(entities.Where(x => x.Id == 0));
            }

            return entities;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteOrRestoreAsync(int id)
    {
        if (id == 0) throw new ArgumentNullException(nameof(id));

        try
        {
            var entity = _context.Find<T>(id);

            if (entity == null) throw new NullReferenceException(nameof(entity));

            entity.IsActive = !entity.IsActive;

            _context.Update(entity);
        }
        catch (Exception)
        {
            throw;
        }

        return true;
    }

    public async Task<bool> DeletePermanentAsync(int id)
    {
        if (id == 0) throw new ArgumentNullException(nameof(id));

        try
        {
            var entity = _context.Find<T>(id);

            if (entity == null) throw new NullReferenceException(nameof(entity));

            _context.Remove(entity);
        }
        catch (Exception)
        {
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}