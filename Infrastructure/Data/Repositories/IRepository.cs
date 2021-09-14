using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{

    namespace Data.Repositories
    {
        public interface IRepository<T> where T : class
        {
            T GetById(int id);
            Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
            IEnumerable<T> GetAll();

            Task<IEnumerable<T>> GetAllAsync(CancellationToken token);

            IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

            void Add(T entity);

            Task AddAsync(T entity, CancellationToken cancellationToken);

            void AddRange(IEnumerable<T> entities);

            void Remove(T entity);

            void RemoveRange(IEnumerable<T> entities);
            IQueryable<T> CreateQuery();
        }
    }

}