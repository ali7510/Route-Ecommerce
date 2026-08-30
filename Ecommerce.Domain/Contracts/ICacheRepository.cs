using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string key, CancellationToken ct = default);
        Task SetAsync(string key, string value, TimeSpan? expiration = null, CancellationToken ct = default);
    }
}
