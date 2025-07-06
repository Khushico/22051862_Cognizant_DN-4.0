using Microsoft.EntityFrameworkCore;
using RetailInventorySystem.Data;
using RetailInventorySystem.Models;

namespace RetailInventorySystem.Services
{
    public class DataSeedingService : IDataSeedingService
    {
        private readonly RetailDbContext _context;
        
        public DataSeedingService(RetailDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> HasDataAsync()
        {
            return await _context.Categories.AnyAsync() || 
                   await _context.Products.AnyAsync() || 
                   await _context.Customers.AnyAsync();
        }
        
        public async Task SeedAllDataAsync()
        {
            if (await HasDataAsync())
            {
                Console.WriteLine("Database already contains data. Skipping seed.");
                return;
            }
            
            await SeedCategoriesAsync();
            await SeedSuppliersAsync();
            await SeedCustomersAsync();
            await SeedProductsAsync();
            await SeedOrdersAsync();
            
            Console.WriteLine("All data seeded successfully!");
        }
        
        public async Task SeedCategoriesAsync()
        {
            if (!await _context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { CategoryName = "Electronics", Description = "Electronic devices and accessories" },
                    new Category { CategoryName = "Clothing", Description = "Apparel and fashion items" },
                    new Category { CategoryName = "Books", Description = "Books and educational materials" },
                    new Category { CategoryName = "Home & Garden", Description = "Home improvement and garden supplies" },
                    new Category { CategoryName = "Sports & Outdoors", Description = "Sports equipment and outdoor gear" }
                };
                
                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Seeded {categories.Count} categories");
            }
        }
        
        public async Task SeedSuppliersAsync()
        {
            if (!await _context.Suppliers.AnyAsync())
            {
                var suppliers = new List<Supplier>
                {
                    new Supplier { SupplierName = "TechCorp Solutions", ContactPerson = "John Smith", Email = "john@techcorp.com", Phone = "555-0101" },
                    new Supplier { SupplierName = "Fashion World Inc", ContactPerson = "Sarah Johnson", Email = "sarah@fashionworld.com", Phone = "555-0102" },
                    new Supplier { SupplierName = "BookMaster Publishing", ContactPerson = "Michael Brown", Email = "michael@bookmaster.com", Phone = "555-0103" },
                    new Supplier { SupplierName = "HomeStyle Distributors", ContactPerson = "Emily Davis", Email = "emily@homestyle.com", Phone = "555-0104" },
                    new Supplier { SupplierName = "SportsPro Equipment", ContactPerson = "David Wilson", Email = "david@sportspro.com", Phone = "555-0105" }
                };
                
                await _context.Suppliers.AddRangeAsync(suppliers);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Seeded {suppliers.Count} suppliers");
            }
        }
        
        public async Task SeedCustomersAsync()
        {
            if (!await _context.Customers.AnyAsync())
            {
                var customers = new List<Customer>
                {
                    new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", Phone = "123-456-7890", Address = "123 Main St" },
                    new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", Phone = "098-765-4321", Address = "456 Oak Ave" },
                    new Customer { FirstName = "Mike", LastName = "Johnson", Email = "mike.johnson@email.com", Phone = "555-123-4567", Address = "789 Pine Rd" },
                    new Customer { FirstName = "Lisa", LastName = "Williams", Email = "lisa.williams@email.com", Phone = "777-888-9999", Address = "111 Elm St" },
                    new Customer { FirstName = "Robert", LastName = "Brown", Email = "robert.brown@email.com", Phone = "444-555-6666", Address = "222 Maple Dr" }
                };
                
                await _context.Customers.AddRangeAsync(customers);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Seeded {customers.Count} customers");
            }
        }
        
        public async Task SeedProductsAsync()
        {
            if (!await _context.Products.AnyAsync())
            {
                var categories = await _context.Categories.ToListAsync();
                var suppliers = await _context.Suppliers.ToListAsync();
                
                var products = new List<Product>
                {
                    new Product { ProductName = "Laptop Pro 15", Description = "High-performance laptop", Price = 1299.99m, CostPrice = 899.99m, StockQuantity = 50, CategoryId = categories[0].CategoryId, SupplierId = suppliers[0].SupplierId, SKU = "SKU-LAP-001" },
                    new Product { ProductName = "Smartphone X", Description = "Latest smartphone", Price = 799.99m, CostPrice = 549.99m, StockQuantity = 100, CategoryId = categories[0].CategoryId, SupplierId = suppliers[0].SupplierId, SKU = "SKU-PHN-002" },
                    new Product { ProductName = "Wireless Headphones", Description = "Bluetooth headphones", Price = 199.99m, CostPrice = 129.99m, StockQuantity = 75, CategoryId = categories[0].CategoryId, SupplierId = suppliers[0].SupplierId, SKU = "SKU-HDP-003" },
                    new Product { ProductName = "Premium T-Shirt", Description = "100% cotton t-shirt", Price = 29.99m, CostPrice = 14.99m, StockQuantity = 200, CategoryId = categories[1].CategoryId, SupplierId = suppliers[1].SupplierId, SKU = "SKU-TSH-004" },
                    new Product { ProductName = "Designer Jeans", Description = "Premium denim jeans", Price = 89.99m, CostPrice = 44.99m, StockQuantity = 150, CategoryId = categories[1].CategoryId, SupplierId = suppliers[1].SupplierId, SKU = "SKU-JNS-005" }
                };
                
                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Seeded {products.Count} products");
            }
        }
        
        public async Task SeedOrdersAsync()
        {
            if (!await _context.Orders.AnyAsync())
            {
                var customers = await _context.Customers.ToListAsync();
                var products = await _context.Products.ToListAsync();
                
                var orders = new List<Order>();
                var orderDetails = new List<OrderDetail>();
                
                // Create sample orders
                for (int i = 0; i < 5; i++)
                {
                    var order = new Order
                    {
                        CustomerId = customers[i % customers.Count].CustomerId,
                        OrderDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 30)),
                        Status = new[] { "Pending", "Processing", "Shipped", "Completed" }[Random.Shared.Next(4)],
                        TotalAmount = 0
                    };
                    orders.Add(order);
                }
                
                await _context.Orders.AddRangeAsync(orders);
                await _context.SaveChangesAsync();
                
                // Create order details
                foreach (var order in orders)
                {
                    var numItems = Random.Shared.Next(1, 4);
                    decimal totalAmount = 0;
                    
                    for (int i = 0; i < numItems; i++)
                    {
                        var product = products[Random.Shared.Next(products.Count)];
                        var quantity = Random.Shared.Next(1, 3);
                        var totalPrice = product.Price * quantity;
                        
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = product.ProductId,
                            Quantity = quantity,
                            UnitPrice = product.Price,
                            TotalPrice = totalPrice
                        };
                        orderDetails.Add(orderDetail);
                        totalAmount += totalPrice;
                    }
                    
                    order.TotalAmount = totalAmount;
                }
                
                await _context.OrderDetails.AddRangeAsync(orderDetails);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Seeded {orders.Count} orders with {orderDetails.Count} order details");
            }
        }
        
        public async Task ClearAllDataAsync()
        {
            await _context.OrderDetails.ExecuteDeleteAsync();
            await _context.Orders.ExecuteDeleteAsync();
            await _context.Products.ExecuteDeleteAsync();
            await _context.Customers.ExecuteDeleteAsync();
            await _context.Suppliers.ExecuteDeleteAsync();
            await _context.Categories.ExecuteDeleteAsync();
            
            Console.WriteLine("All data cleared successfully!");
        }
    }
}