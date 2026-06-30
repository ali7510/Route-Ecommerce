using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs.ProductDTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string PictureURL { get; set; } = null!;
        public string ProductType { get; set; } = null!;
        public string ProductBrand { get; set; } = null!;
    }
}
