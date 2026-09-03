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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Subtotal).HasPrecision(8, 2);
            builder.OwnsOne(o=>o.Address, a =>
            {
                a.Property(x => x.FirstName).HasMaxLength(50);
                a.Property(x=>x.LastName).HasMaxLength(50);
                a.Property(x=>x.City).HasMaxLength(50);
                a.Property(x => x.Street).HasMaxLength(50);
                a.Property(x=>x.Country).HasMaxLength(50);
            });
        }
    }
}
