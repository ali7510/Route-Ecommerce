using Ecommerce.Domain.Contracts;
using Ecommerce.Persistence.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            if (dbContextService.Database.GetPendingMigrations().Any())
            {
                await dbContextService.Database.MigrateAsync();
            }
            return app;
        }

        public static async Task<WebApplication> MigrateIdentityDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            if (dbContextService.Database.GetPendingMigrations().Any())
            {
                await dbContextService.Database.MigrateAsync();
            }
            return app;
        }

        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var dataInitializeService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Default");
            await dataInitializeService.initializeDataAsync();
            return app;
        }

        public static async Task<WebApplication> SeedIdentityDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var dataInitializeService = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Identity");
            await dataInitializeService.initializeDataAsync();
            return app;
        }
    }
}
