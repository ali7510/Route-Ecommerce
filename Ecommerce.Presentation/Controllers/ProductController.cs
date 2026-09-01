using Ecommerce.Presentation.Attributes;
using Ecommerce.ServiceAbstraction.ProductServicesAbstraction;
using Ecommerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    
    public class ProductController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [RedisCache]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync([FromQuery]ProductQueryParams? productParams)
        {
            var products = await _productService.GetAllProductsAsync(productParams);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
        {
            var Result = await _productService.GetProductByIdAsync(id);
            return HandleResult<ProductDto>(Result);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllProductTypesAsync()
        {
            var types = await _productService.GetAllProductTypesAsync();
            return Ok(types);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllProductBrandsAsync()
        {
            var brands = await _productService.GetAllProductBrandsAsync();
            return Ok(brands);
        }
    }
}
