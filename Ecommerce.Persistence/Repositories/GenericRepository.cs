using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Persistence.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Repositories
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;
        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);

        }

        public async Task<IEnumerable<T>?> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(TKey id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}
