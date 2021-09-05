using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstraction.DDD;
using Data.Context;
using Data.Repositories;
using Data.Repositories.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContextBase 
    {
        private bool _disposed = false;
        private readonly TContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        
        public UnitOfWork()
        {
            Type type = typeof(TContext);
            object result = Activator.CreateInstance(type);
            _context = (TContext)result;
        
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;
            }

            IRepository<TEntity> repository = new Repository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }


        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Rollback()
        {
            _context
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}