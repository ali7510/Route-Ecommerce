using Ecommerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p=>p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p=>p.PictureURL).HasMaxLength(200);
            builder.Property(p => p.Price).HasPrecision(18, 2);


            #region Relationships
            builder.HasOne(p=>p.ProductBrand)
                .WithMany()
                .HasForeignKey(p => p.ProductBrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p=>p.ProductType)
                .WithMany()
                .HasForeignKey(p=>p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
