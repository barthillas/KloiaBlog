using System;
using System.Threading.Tasks;
using Abstraction.DDD;
using Data.Repositories.Data.Repositories;

namespace Data.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IDisposable
        
    {
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
        public Task<int> Complete();
    }
}