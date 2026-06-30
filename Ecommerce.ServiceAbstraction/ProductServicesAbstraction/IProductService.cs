using Ecommerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction.ProductServicesAbstraction
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>?> GetAllProductsAsync();
        public Task<ProductDto?> GetProductByIdAsync(int id);
        public Task<IEnumerable<TypeDto>?> GetAllProductTypesAsync();
        public Task<IEnumerable<BrandDto>?> GetAllProductBrandsAsync();
    }
}
