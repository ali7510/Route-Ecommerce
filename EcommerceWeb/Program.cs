
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.IdentityModule;
using Ecommerce.Persistence.Data.DataSeed;
using Ecommerce.Persistence.Data.DBContext;
using Ecommerce.Persistence.IdentityData.DataSeed;
using Ecommerce.Persistence.IdentityData.DbContext;
using Ecommerce.Persistence.NoSqlDbSettings;
using Ecommerce.Persistence.Repositories;
using Ecommerce.Service;
using Ecommerce.Service.MappingProfiles;
using Ecommerce.Service.ProductServices;
using Ecommerce.ServiceAbstraction;
using Ecommerce.ServiceAbstraction.ProductServicesAbstraction;
using EcommerceWeb.CustomMiddleware;
using EcommerceWeb.Extensions;
using EcommerceWeb.Factory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

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

            builder.Services.AddKeyedScoped<IDataInitializer, DataInitializer>("Default");
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("Identity");
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>();
            builder.Services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            //builder.Services.AddTransient<ProductPictureUrlResolver>();
            //builder.Services.AddAutoMapper(x=>x.AddProfile<ProductProfile>());
            builder.Services.AddAutoMapper(typeof(ServiceAssemblyReference).Assembly);
            builder.Services.AddScoped<MongoIndexInitializer>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // how to authenticate? (Bearer)
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
            options.SaveToken = true;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                ValidAudience = builder.Configuration["JwtOptions:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"]!))
            };});

            // to visualize the authorization button in swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            var app = builder.Build();



            #region Data Seeding
            await app.MigrateDatabaseAsync();
            await app.MigrateIdentityDatabaseAsync();
            await app.SeedDataAsync();
            await app.SeedIdentityDataAsync();
            #endregion

            #region Pipeline

            //app.Use(async(context, next) =>
            //{
            //    try
            //    {
            //        await next.Invoke(context);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            //        // returning the response
            //        await context.Response.WriteAsJsonAsync(new
            //        {
            //            StatusCode = StatusCodes.Status500InternalServerError,
            //            Error = ex.Message, 
            //        });
            //    }
            //});

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<MongoIndexInitializer>();
                await initializer.InitializeAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
