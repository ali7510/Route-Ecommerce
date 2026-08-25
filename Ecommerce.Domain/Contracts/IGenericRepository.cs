using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        Task<IEnumerable<T>?> GetAllAsync();
        Task<IEnumerable<T>?> GetAllAsync(ISpecifications<T, TKey> specifications);
        Task<T?> GetByIdAsync(TKey id);
        Task<T?> GetByIdAsync(TKey id, ISpecifications<T, TKey> specifications);
        Task AddAsync(T entity);

        void Remove(T entity);
        void Update(T entity);
    }
}
