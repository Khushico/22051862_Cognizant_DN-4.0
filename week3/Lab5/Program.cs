using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Memory;
using RetailInventorySystem.Data;
using RetailInventorySystem.Services;

namespace RetailInventorySystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== EF Core Lab 5: Retrieving Data Demo ===");
            
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            // Setup dependency injection
            var services = new ServiceCollection();
            
            // Add DbContext
            services.AddDbContext<RetailDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            // Add memory cache
            services.AddMemoryCache();
            
            // Add all services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IMigrationService, MigrationService>();
            services.AddScoped<IDataSeedingService, DataSeedingService>();
            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<AdvancedQueryExamples>();
            services.AddScoped<IQueryOptimizationService, QueryOptimizationService>();
            services.AddScoped<IQueryCachingService, QueryCachingService>();
            services.AddScoped<JsonDataSeeder>();
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Run Lab 5 demonstrations
            using (var scope = serviceProvider.CreateScope())
            {
                await EnsureDatabaseAndData(scope.ServiceProvider);
                await Lab5Demo.RunQueryDemonstrations(scope.ServiceProvider);
                await RunOptimizationDemo(scope.ServiceProvider);
                await RunCachingDemo(scope.ServiceProvider);
            }
        }
        
        static async Task EnsureDatabaseAndData(IServiceProvider serviceProvider)
        {
            var migrationService = serviceProvider.GetRequiredService<IMigrationService>();
            var dataSeedingService = serviceProvider.GetRequiredService<IDataSeedingService>();
            
            // Ensure database is up to date
            await migrationService.ApplyMigrationsAsync();
            
            // Ensure we have data to query
            if (!await dataSeedingService.HasDataAsync())
            {
                await dataSeedingService.SeedAllDataAsync();
            }
        }
        
        static async Task RunOptimizationDemo(IServiceProvider serviceProvider)
        {
            var optimizationService = serviceProvider.GetRequiredService<IQueryOptimizationService>();
            await optimizationService.DemonstrateQueryOptimizationAsync();
        }
        
        static async Task RunCachingDemo(IServiceProvider serviceProvider)
        {
            var cachingService = serviceProvider.GetRequiredService<IQueryCachingService>();
            await cachingService.DemonstrateCachingPerformanceAsync();
        }
    }
}