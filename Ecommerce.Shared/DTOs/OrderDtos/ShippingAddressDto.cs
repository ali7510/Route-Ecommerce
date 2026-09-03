using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs.OrderDtos
{
    public record ShippingAddressDto(string firstName, string lastName, string country, string street, string city);
}
