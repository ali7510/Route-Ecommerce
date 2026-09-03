using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.OrderModule
{
    public enum OrderStatus
    {
        Pending,
        PaymentRecieved,
        PaymentFailed,
    }
}
