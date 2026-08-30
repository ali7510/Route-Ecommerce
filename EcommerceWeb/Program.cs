
using Ecommerce.Domain.Contracts;
using Ecommerce.Persistence.Data.DataSeed;
using Ecommerce.Persistence.Data.DBContext;
using Ecommerce.Persistence.NoSqlDbSettings;
using Ecommerce.Persistence.Repositories;
using Ecommerce.Service;
using Ecommerce.Service.MappingProfiles;
using Ecommerce.Service.ProductServices;
using Ecommerce.ServiceAbstraction;
using Ecommerce.ServiceAbstraction.ProductServicesAbstraction;
using EcommerceWeb.Extensions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

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

            // MongoDB Settings
            builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection("MongoDbSettings")
            );

            // MongoDB Client
            //builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
            //{
            //    var configuration = serviceProvider
            //        .GetRequiredService<IConfiguration>();

            //    var connectionString = configuration["MongoDbSettings:ConnectionString"];

            //    return new MongoClient(connectionString);
            //});
            builder.Services.AddSingleton<MongoContext>();
            builder.Services.AddMemoryCache();

            // MongoDB Database
            builder.Services.AddScoped<IMongoDatabase>(serviceProvider =>
            {
                var configuration = serviceProvider
                    .GetRequiredService<IConfiguration>();

                var client = serviceProvider.GetRequiredService<IMongoClient>();

                var databaseName = configuration["MongoDbSettings:DatabaseName"];

                return client.GetDatabase(databaseName);
            });

            builder.Services.AddScoped<IDataInitializer, DataInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            //builder.Services.AddTransient<ProductPictureUrlResolver>();
            //builder.Services.AddAutoMapper(x=>x.AddProfile<ProductProfile>());
            builder.Services.AddAutoMapper(typeof(ServiceAssemblyReference).Assembly);
            builder.Services.AddScoped<MongoIndexInitializer>();
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<MongoIndexInitializer>();
                await initializer.InitializeAsync();
            }

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
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
