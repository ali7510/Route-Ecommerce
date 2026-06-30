using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangeAsync();

        public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey>;
    }
}
