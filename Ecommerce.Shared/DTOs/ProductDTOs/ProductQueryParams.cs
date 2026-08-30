using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs.ProductDTOs
{
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? SearchWord { get; set; }
        public ProductSortingOptions? SortingOption { get; set; }

        private int _pageIndex = 1;
        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        private int _pageSize = 5;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value <= 0)
                {
                    _pageSize = DefaultPageSize;
                }
                else if (value > MaxPageSize)
                {
                    _pageSize = MaxPageSize;
                }
                else
                {
                    _pageSize = value;
                }
            }
        }
        public int PageIndex
        {
            get {  return _pageIndex; }
            set
            {
                _pageIndex = (value <= 0)? 1 : value;
            }

        }
    }
}
