using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.ProductModule
{
    public class ProductType : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
    }
}
