using Ecommerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Data.Configurations
{
    public class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(d=>d.Price).HasPrecision(8, 2);
            builder.Property(d=>d.ShortName).HasMaxLength(50);
            builder.Property(d=>d.DeliveryTime).HasMaxLength(50);
            builder.Property(d=>d.Description).HasMaxLength(50);
        }
    }
}
