using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.OrderModule;
using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Persistence.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task initializeDataAsync()
        {
            try
            {
                var hasProducts = await _dbContext.Set<Product>().AnyAsync();
                var hasProductBrands = await _dbContext.Set<ProductBrand>().AnyAsync();
                var hasProductTypes = await _dbContext.Set<ProductType>().AnyAsync();
                var hasDeliveryMethods = await _dbContext.Set<DeliveryMethod>().AnyAsync();

                if (!hasProductBrands)
                {
                    await SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.Set<ProductBrand>());
                    await _dbContext.SaveChangesAsync();
                }
                if (!hasProductTypes)
                {
                    await SeedDataFromJson<ProductType, int>("types.json", _dbContext.Set<ProductType>());
                    await _dbContext.SaveChangesAsync();
                }
                if (!hasProducts)
                {
                    await SeedDataFromJson<Product, int>("products.json", _dbContext.Set<Product>());
                    await _dbContext.SaveChangesAsync();

                }
                if (!hasDeliveryMethods)
                {
                    await SeedDataFromJson<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error initializing data: {ex.Message}", ex);

            }
        }


        private async Task SeedDataFromJson<T, TKey>(string fineName, DbSet<T> dbSet) where T : BaseEntity<TKey>
        {
            var path = @"..\Ecommerce.Persistence\Data\DataSeed\JSONFiles\" + fineName;
            if (!File.Exists(path)) throw new FileNotFoundException($"File not found: {path}");
            try
            {
                using var dataStream = File.OpenRead(path);
                var data = JsonSerializer.Deserialize<List<T>>(dataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                if (data != null && data.Count > 0)
                {
                    await dbSet.AddRangeAsync(data);
                    
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Error seeding data from file {fineName}: {ex.Message}", ex);
            }
        }
    }
}
