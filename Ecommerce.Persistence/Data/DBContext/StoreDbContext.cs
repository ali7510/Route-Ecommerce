using Ecommerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ecommerce.Persistence.Data.DBContext
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // the same assembly (Project)
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductBrand> ProductBrands { get; set; } = null!;
        public DbSet<ProductType> ProductTypes { get; set; } = null!;
    }
}
