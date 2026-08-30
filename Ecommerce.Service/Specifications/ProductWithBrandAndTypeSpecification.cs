using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product, int>
    {
        //public ProductWithBrandAndTypeSpecification() : base()
        //{
        //    AddInclude(p=>p.ProductBrand);
        //    AddInclude(p => p.ProductType);
        //}

        public ProductWithBrandAndTypeSpecification(ProductQueryParams? productParams) : base()
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            if (productParams is not null)
            {
                if (productParams.BrandId.HasValue)
                {
                    AddCondition(p => p.ProductBrandId == productParams.BrandId.Value);
                }
                if (productParams.TypeId.HasValue)
                {
                    AddCondition(p => p.ProductTypeId == productParams.TypeId.Value);
                }
                if (!string.IsNullOrEmpty(productParams.SearchWord))
                {
                    AddCondition(p => p.Name.ToLower().Contains(productParams.SearchWord.ToLower()));
                }
                if (productParams.SortingOption.HasValue)
                {
                    switch (productParams.SortingOption.Value)
                    {
                        case ProductSortingOptions.PriceAsc:
                            AddOrderBy(p => p.Price);
                            break;
                        case ProductSortingOptions.PriceDesc:
                            AddOrderByDescending(p => p.Price);
                            break;
                        case ProductSortingOptions.NameAsc:
                            AddOrderBy(p => p.Name);
                            break;
                        case ProductSortingOptions.NameDesc:
                            AddOrderByDescending(p => p.Name);
                            break;
                        default:
                            AddOrderBy(p => p.Name);
                            break;
                    }
                }
                ApplyPagination(productParams.PageSize, productParams.PageIndex);
            }

        }
    }
}
