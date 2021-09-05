using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Data.Repositories
{

    namespace Data.Repositories
    {
        public interface IRepository<T> where T : class
        {
            T GetById(int id);

            IEnumerable<T> GetAll();

            Task<IEnumerable<T>> GetAllAsync(CancellationToken token);

            IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

            void Add(T entity);

            Task AddAsync(T entity, CancellationToken cancellationToken);

            void AddRange(IEnumerable<T> entities);

            void Remove(T entity);

            void RemoveRange(IEnumerable<T> entities);
            IQueryable<T> CreateQuery(Expression<Func<T, bool>> predicate = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                bool disableTracking = true);
        }
    }

}