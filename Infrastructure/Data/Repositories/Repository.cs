using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Data.Repositories.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _entities = context.Set<T>();
        }

        public T GetById(int id)
        {
            return _entities.Find(id);
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        {
            return await _entities.FirstOrDefaultAsync(predicate: predicate, token).ConfigureAwait(false);
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }
        
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token)
        {
            return await _entities.ToListAsync(token);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }
        
        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _entities.AddRange(entities);
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }
        
        public IQueryable<T> CreateQuery() 
        {
            var query = _entities.AsQueryable();
           
            return query;
        }

    }
}
