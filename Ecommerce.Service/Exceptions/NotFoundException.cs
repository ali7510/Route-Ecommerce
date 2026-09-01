using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Exceptions
{
    public abstract class NotFoundException(string message) : Exception(message)
    {
        
    }

    public sealed class ProductNotFoundException(int productId) : NotFoundException($"Product with ID {productId} was not found.")
    {
    }

    public sealed class BasketNotFoundException(string basketId) : NotFoundException($"Basket with ID {basketId} was not found.")
    {
    }
}
