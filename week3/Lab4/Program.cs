using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailInventorySystem.Data;
using RetailInventorySystem.Models;
using RetailInventorySystem.Services;

namespace RetailInventorySystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== EF Core Labs 3 & 4 Demo ===");
            
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
            
            // Add services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IMigrationService, MigrationService>();
            services.AddScoped<IDataSeedingService, DataSeedingService>();
            services.AddScoped<JsonDataSeeder>();
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Run demos
            using (var scope = serviceProvider.CreateScope())
            {
                await RunLab3Demo(scope.ServiceProvider);
                await RunLab4Demo(scope.ServiceProvider);
                await DemonstrateDataOperations(scope.ServiceProvider);
            }
        }
        
        static async Task RunLab3Demo(IServiceProvider serviceProvider)
        {
            Console.WriteLine("\n=== LAB 3: Migration Demo ===");
            
            var migrationService = serviceProvider.GetRequiredService<IMigrationService>();
            
            // Check migration status
            var isUpToDate = await migrationService.IsDatabaseUpToDateAsync();
            Console.WriteLine($"Database up to date: {isUpToDate}");
            
            // Get pending migrations
            var pendingMigrations = await migrationService.GetPendingMigrationsAsync();
            Console.WriteLine($"Pending migrations: {(pendingMigrations.Any() ? string.Join(", ", pendingMigrations) : "None")}");
            
            // Get applied migrations
            var appliedMigrations = await migrationService.GetAppliedMigrationsAsync();
            Console.WriteLine($"Applied migrations: {(appliedMigrations.Any() ? string.Join(", ", appliedMigrations) : "None")}");
            
            // Apply pending migrations if any
            if (pendingMigrations.Any())
            {
                Console.WriteLine("Applying pending migrations...");
                await migrationService.ApplyMigrationsAsync();
                Console.WriteLine("✅ Migrations applied successfully!");
            }
            else
            {
                Console.WriteLine("✅ Database is up to date!");
            }
        }
        
        static async Task RunLab4Demo(IServiceProvider serviceProvider)
        {
            Console.WriteLine("\n=== LAB 4: Data Seeding Demo ===");
            
            var dataSeedingService = serviceProvider.GetRequiredService<IDataSeedingService>();
            var jsonDataSeeder = serviceProvider.GetRequiredService<JsonDataSeeder>();
            
            // Check if data exists
            var hasData = await dataSeedingService.HasDataAsync();
            Console.WriteLine($"Database has existing data: {hasData}");
            
            if (!hasData)
            {
                Console.WriteLine("Seeding initial data...");
                await dataSeedingService.SeedAllDataAsync();
                Console.WriteLine("✅ Data seeding completed!");
            }
            else
            {
                Console.WriteLine("Database already contains data. Seeding skipped.");
            }
            
            // Demonstrate JSON seeding (optional)
            Console.WriteLine("\nDemonstrating JSON data seeding capability...");
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "sample-data.json");
            if (File.Exists(jsonFilePath))
            {
                await jsonDataSeeder.SeedFromJsonAsync(jsonFilePath);
            }
            else
            {
                Console.WriteLine("sample-data.json not found. Skipping JSON seed demo.");
            }
        }
        
        static async Task DemonstrateDataOperations(IServiceProvider serviceProvider)
        {
            Console.WriteLine("\n=== Data Operations Demo ===");
            
            var context = serviceProvider.GetRequiredService<RetailDbContext>();
            
            // Display seeded data statistics
            var categoriesCount = await context.Categories.CountAsync();
            var suppliersCount = await context.Suppliers.CountAsync();
            var customersCount = await context.Customers.CountAsync();
            var productsCount = await context.Products.CountAsync();
            var ordersCount = await context.Orders.CountAsync();
            
            Console.WriteLine($"📊 Database Statistics:");
            Console.WriteLine($"   Categories: {categoriesCount}");
            Console.WriteLine($"   Suppliers: {suppliersCount}");
            Console.WriteLine($"   Customers: {customersCount}");
            Console.WriteLine($"   Products: {productsCount}");
            Console.WriteLine($"   Orders: {ordersCount}");
            
            // Display sample data with relationships
            Console.WriteLine("\n📦 Sample Products with Categories and Suppliers:");
            var products = await context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Take(5)
                .ToListAsync();
            
            foreach (var product in products)
            {
                Console.WriteLine($"   • {product.ProductName} ({product.SKU})");
                Console.WriteLine($"     Category: {product.Category.CategoryName}");
                Console.WriteLine($"     Supplier: {product.Supplier?.SupplierName ?? "No Supplier"}");
                Console.WriteLine($"     Price: ${product.Price:F2} | Stock: {product.StockQuantity}");
                Console.WriteLine();
            }
            
            // Display recent orders
            Console.WriteLine("🛒 Recent Orders:");
            var recentOrders = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderDate)
                .Take(3)
                .ToListAsync();
            
            foreach (var order in recentOrders)
            {
                Console.WriteLine($"   Order #{order.OrderId} - {order.Customer.FirstName} {order.Customer.LastName}");
                Console.WriteLine($"   Date: {order.OrderDate:yyyy-MM-dd} | Status: {order.Status} | Total: ${order.TotalAmount:F2}");
                Console.WriteLine($"   Items: {string.Join(", ", order.OrderDetails.Select(od => $"{od.Product.ProductName} (x{od.Quantity})"))}");
                Console.WriteLine();
            }
        }
    }
}