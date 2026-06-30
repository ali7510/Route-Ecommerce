
using Ecommerce.Domain.Contracts;
using Ecommerce.Persistence.Data.DataSeed;
using Ecommerce.Persistence.Data.DBContext;
using Ecommerce.Persistence.Repositories;
using Ecommerce.Service.MappingProfiles;
using Ecommerce.Service.ProductServices;
using Ecommerce.ServiceAbstraction.ProductServicesAbstraction;
using EcommerceWeb.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

            builder.Services.AddScoped<IDataInitializer, DataInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddAutoMapper(x=>x.AddProfile<ProductProfile>());
            var app = builder.Build();

            #region Data Seeding
            await app.MigrateDatabaseAsync();
            await app.SeedDataAsync();
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
