using Ecommerce.Domain.Contracts;
using Ecommerce.Persistence.NoSqlDbSettings;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IMemoryCache _cache;

        public CacheRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<string?> GetAsync(string key, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var cachedValue = _cache.Get(key);
            return Task.FromResult(cachedValue as string);
        }

        public Task SetAsync(string key, string value, TimeSpan? expiration = null, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30) // Default expiration of 30 minutes
            };
            _cache.Set(key, value, cacheEntryOptions);
            return Task.CompletedTask;
        }
    }
}
