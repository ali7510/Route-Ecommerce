using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Persistence.Data.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object?> _repositories = new Dictionary<Type, object?>();

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey>
        {
            if(!_repositories.ContainsKey(typeof(T)))
            {
                var repository = new GenericRepository<T, TKey>(_dbContext);
                _repositories.Add(typeof(T), repository);
            }
            return (IGenericRepository<T, TKey>)_repositories[typeof(T)]!;
        }

        public async Task<int> SaveChangeAsync() => await _dbContext.SaveChangesAsync();
    }
}
