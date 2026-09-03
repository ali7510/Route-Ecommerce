using Ecommerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.Price).HasPrecision(8, 2);
            builder.OwnsOne(oi=> oi.Product, p =>
            {
                p.Property(x => x.ProductName).HasMaxLength(100);
                p.Property(x => x.PictureUrl).HasMaxLength(200);
            });
        }
    }
}
