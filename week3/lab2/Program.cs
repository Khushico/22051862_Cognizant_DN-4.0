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
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Ensure database is created
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RetailDbContext>();
                await context.Database.EnsureCreatedAsync();
                
                Console.WriteLine("Database initialized successfully!");
                
                // Demonstrate CRUD operations
                await DemonstrateCRUDOperations(scope.ServiceProvider);
            }
        }
        
        static async Task DemonstrateCRUDOperations(IServiceProvider serviceProvider)
        {
            var productService = serviceProvider.GetRequiredService<IProductService>();
            var customerService = serviceProvider.GetRequiredService<ICustomerService>();
            var orderService = serviceProvider.GetRequiredService<IOrderService>();
            
            Console.WriteLine("\n=== CRUD Operations Demo ===");
            
            // Read operations
            Console.WriteLine("\n1. Reading all products:");
            var products = await productService.GetAllProductsAsync();
            foreach (var product in products)
            {
                Console.WriteLine($"- {product.ProductName}: ${product.Price} (Stock: {product.StockQuantity})");
            }
            
            // Create operation
            Console.WriteLine("\n2. Creating a new product:");
            var newProduct = new Product
            {
                ProductName = "Wireless Headphones",
                Description = "Bluetooth wireless headphones",
                Price = 129.99m,
                StockQuantity = 80,
                CategoryId = 1
            };
            
            var createdProduct = await productService.CreateProductAsync(newProduct);
            Console.WriteLine($"Created: {createdProduct.ProductName} with ID: {createdProduct.ProductId}");
            
            // Update operation
            Console.WriteLine("\n3. Updating product price:");
            createdProduct.Price = 119.99m;
            await productService.UpdateProductAsync(createdProduct);
            Console.WriteLine($"Updated price to: ${createdProduct.Price}");
            
            // Create and process an order
            Console.WriteLine("\n4. Creating a new order:");
            var order = new Order
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Status = "Processing"
            };
            
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail { ProductId = 1, Quantity = 1, UnitPrice = 999.99m, TotalPrice = 999.99m },
                new OrderDetail { ProductId = 3, Quantity = 2, UnitPrice = 19.99m, TotalPrice = 39.98m }
            };
            
            var createdOrder = await orderService.CreateOrderAsync(order, orderDetails);
            Console.WriteLine($"Created order with ID: {createdOrder.OrderId}, Total: ${createdOrder.TotalAmount}");
            
            Console.WriteLine("\n=== Demo completed successfully! ===");
        }
    }
}