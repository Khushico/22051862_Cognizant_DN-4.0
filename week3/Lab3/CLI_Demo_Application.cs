using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RetailInventorySystem.Data;
using RetailInventorySystem.Services;

namespace RetailInventorySystem
{
    public class MigrationDemo
    {
        public static async Task RunMigrationDemo(IServiceProvider serviceProvider)
        {
            var migrationService = serviceProvider.GetRequiredService<IMigrationService>();
            
            Console.WriteLine("=== Migration Status Demo ===");
            
            // Check if database is up to date
            var isUpToDate = await migrationService.IsDatabaseUpToDateAsync();
            Console.WriteLine($"Database up to date: {isUpToDate}");
            
            // Get pending migrations
            var pendingMigrations = await migrationService.GetPendingMigrationsAsync();
            Console.WriteLine($"Pending migrations: {string.Join(", ", pendingMigrations)}");
            
            // Get applied migrations
            var appliedMigrations = await migrationService.GetAppliedMigrationsAsync();
            Console.WriteLine($"Applied migrations: {string.Join(", ", appliedMigrations)}");
            
            if (pendingMigrations.Any())
            {
                Console.WriteLine("Applying pending migrations...");
                await migrationService.ApplyMigrationsAsync();
                Console.WriteLine("Migrations applied successfully!");
            }
        }
    }
}