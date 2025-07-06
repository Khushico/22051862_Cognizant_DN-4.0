using Microsoft.EntityFrameworkCore;
using RetailInventorySystem.Models;

namespace RetailInventorySystem.Data
{
    public class RetailDbContext : DbContext
    {
        public RetailDbContext(DbContextOptions<RetailDbContext> options) : base(options)
        {
        }
        
        // DbSets for each entity
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure relationships
            ConfigureRelationships(modelBuilder);
            
            // Seed comprehensive data
            SeedAllData(modelBuilder);
        }
        
        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Product-Category relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Product-Supplier relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Customer-Order relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Order-OrderDetail relationship
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Product-OrderDetail relationship
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        
        private void SeedAllData(ModelBuilder modelBuilder)
        {
            SeedCategories(modelBuilder);
            SeedSuppliers(modelBuilder);
            SeedCustomers(modelBuilder);
            SeedProducts(modelBuilder);
            SeedOrders(modelBuilder);
            SeedOrderDetails(modelBuilder);
        }
        
        private void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category 
                { 
                    CategoryId = 1, 
                    CategoryName = "Electronics", 
                    Description = "Electronic devices and accessories",
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new Category 
                { 
                    CategoryId = 2, 
                    CategoryName = "Clothing", 
                    Description = "Apparel and fashion items",
                    CreatedDate = DateTime.Now.AddDays(-25)
                },
                new Category 
                { 
                    CategoryId = 3, 
                    CategoryName = "Books", 
                    Description = "Books and educational materials",
                    CreatedDate = DateTime.Now.AddDays(-20)
                },
                new Category 
                { 
                    CategoryId = 4, 
                    CategoryName = "Home & Garden", 
                    Description = "Home improvement and garden supplies",
                    CreatedDate = DateTime.Now.AddDays(-15)
                },
                new Category 
                { 
                    CategoryId = 5, 
                    CategoryName = "Sports & Outdoors", 
                    Description = "Sports equipment and outdoor gear",
                    CreatedDate = DateTime.Now.AddDays(-10)
                }
            );
        }
        
        private void SeedSuppliers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    SupplierId = 1,
                    SupplierName = "TechCorp Solutions",
                    ContactPerson = "John Smith",
                    Email = "john@techcorp.com",
                    Phone = "555-0101",
                    Address = "123 Tech Street, Silicon Valley, CA",
                    CreatedDate = DateTime.Now.AddDays(-45)
                },
                new Supplier
                {
                    SupplierId = 2,
                    SupplierName = "Fashion World Inc",
                    ContactPerson = "Sarah Johnson",
                    Email = "sarah@fashionworld.com",
                    Phone = "555-0102",
                    Address = "456 Fashion Ave, New York, NY",
                    CreatedDate = DateTime.Now.AddDays(-40)
                },
                new Supplier
                {
                    SupplierId = 3,
                    SupplierName = "BookMaster Publishing",
                    ContactPerson = "Michael Brown",
                    Email = "michael@bookmaster.com",
                    Phone = "555-0103",
                    Address = "789 Literature Lane, Boston, MA",
                    CreatedDate = DateTime.Now.AddDays(-35)
                },
                new Supplier
                {
                    SupplierId = 4,
                    SupplierName = "HomeStyle Distributors",
                    ContactPerson = "Emily Davis",
                    Email = "emily@homestyle.com",
                    Phone = "555-0104",
                    Address = "321 Home Blvd, Chicago, IL",
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new Supplier
                {
                    SupplierId = 5,
                    SupplierName = "SportsPro Equipment",
                    ContactPerson = "David Wilson",
                    Email = "david@sportspro.com",
                    Phone = "555-0105",
                    Address = "654 Sports Way, Denver, CO",
                    CreatedDate = DateTime.Now.AddDays(-25)
                }
            );
        }
        
        private void SeedCustomers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer 
                { 
                    CustomerId = 1, 
                    FirstName = "John", 
                    LastName = "Doe", 
                    Email = "john.doe@email.com", 
                    Phone = "123-456-7890", 
                    Address = "123 Main St, Anytown, USA",
                    CreatedDate = DateTime.Now.AddDays(-60)
                },
                new Customer 
                { 
                    CustomerId = 2, 
                    FirstName = "Jane", 
                    LastName = "Smith", 
                    Email = "jane.smith@email.com", 
                    Phone = "098-765-4321", 
                    Address = "456 Oak Ave, Somewhere, USA",
                    CreatedDate = DateTime.Now.AddDays(-55)
                },
                new Customer 
                { 
                    CustomerId = 3, 
                    FirstName = "Mike", 
                    LastName = "Johnson", 
                    Email = "mike.johnson@email.com", 
                    Phone = "555-123-4567", 
                    Address = "789 Pine Rd, Elsewhere, USA",
                    CreatedDate = DateTime.Now.AddDays(-50)
                },
                new Customer 
                { 
                    CustomerId = 4, 
                    FirstName = "Lisa", 
                    LastName = "Williams", 
                    Email = "lisa.williams@email.com", 
                    Phone = "777-888-9999", 
                    Address = "111 Elm St, Nowhere, USA",
                    CreatedDate = DateTime.Now.AddDays(-45)
                },
                new Customer 
                { 
                    CustomerId = 5, 
                    FirstName = "Robert", 
                    LastName = "Brown", 
                    Email = "robert.brown@email.com", 
                    Phone = "444-555-6666", 
                    Address = "222 Maple Dr, Anyplace, USA",
                    CreatedDate = DateTime.Now.AddDays(-40)
                }
            );
        }
        
        private void SeedProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                // Electronics
                new Product 
                { 
                    ProductId = 1, 
                    ProductName = "Laptop Pro 15", 
                    Description = "High-performance laptop with 16GB RAM", 
                    Price = 1299.99m, 
                    CostPrice = 899.99m,
                    StockQuantity = 50, 
                    CategoryId = 1, 
                    SupplierId = 1,
                    SKU = "SKU-LAP-001",
                    CreatedDate = DateTime.Now.AddDays(-30),
                    LastUpdated = DateTime.Now.AddDays(-5)
                },
                new Product 
                { 
                    ProductId = 2, 
                    ProductName = "Smartphone X", 
                    Description = "Latest smartphone with advanced camera", 
                    Price = 799.99m, 
                    CostPrice = 549.99m,
                    StockQuantity = 100, 
                    CategoryId = 1, 
                    SupplierId = 1,
                    SKU = "SKU-PHN-002",
                    CreatedDate = DateTime.Now.AddDays(-25)
                },
                new Product 
                { 
                    ProductId = 3, 
                    ProductName = "Wireless Headphones", 
                    Description = "Bluetooth noise-cancelling headphones", 
                    Price = 199.99m, 
                    CostPrice = 129.99m,
                    StockQuantity = 75, 
                    CategoryId = 1, 
                    SupplierId = 1,
                    SKU = "SKU-HDP-003",
                    CreatedDate = DateTime.Now.AddDays(-20)
                },
                
                // Clothing
                new Product 
                { 
                    ProductId = 4, 
                    ProductName = "Premium T-Shirt", 
                    Description = "100% cotton premium t-shirt", 
                    Price = 29.99m, 
                    CostPrice = 14.99m,
                    StockQuantity = 200, 
                    CategoryId = 2, 
                    SupplierId = 2,
                    SKU = "SKU-TSH-004",
                    CreatedDate = DateTime.Now.AddDays(-18)
                },
                new Product 
                { 
                    ProductId = 5, 
                    ProductName = "Designer Jeans", 
                    Description = "Premium denim jeans with perfect fit", 
                    Price = 89.99m, 
                    CostPrice = 44.99m,
                    StockQuantity = 150, 
                    CategoryId = 2, 
                    SupplierId = 2,
                    SKU = "SKU-JNS-005",
                    CreatedDate = DateTime.Now.AddDays(-15)
                },
                
                // Books
                new Product 
                { 
                    ProductId = 6, 
                    ProductName = "C# Programming Guide", 
                    Description = "Complete guide to C# programming", 
                    Price = 49.99m, 
                    CostPrice = 29.99m,
                    StockQuantity = 75, 
                    CategoryId = 3, 
                    SupplierId = 3,
                    SKU = "SKU-BKS-006",
                    CreatedDate = DateTime.Now.AddDays(-12)
                },
                new Product 
                { 
                    ProductId = 7, 
                    ProductName = "Database Design Fundamentals", 
                    Description = "Learn database design principles", 
                    Price = 59.99m, 
                    CostPrice = 34.99m,
                    StockQuantity = 60, 
                    CategoryId = 3, 
                    SupplierId = 3,
                    SKU = "SKU-BKS-007",
                    CreatedDate = DateTime.Now.AddDays(-10)
                },
                
                // Home & Garden
                new Product 
                { 
                    ProductId = 8, 
                    ProductName = "Smart Home Hub", 
                    Description = "Central hub for smart home devices", 
                    Price = 149.99m, 
                    CostPrice = 99.99m,
                    StockQuantity = 40, 
                    CategoryId = 4, 
                    SupplierId = 4,
                    SKU = "SKU-HME-008",
                    CreatedDate = DateTime.Now.AddDays(-8)
                },
                
                // Sports & Outdoors
                new Product 
                { 
                    ProductId = 9, 
                    ProductName = "Professional Tennis Racket", 
                    Description = "High-quality tennis racket for professionals", 
                    Price = 129.99m, 
                    CostPrice = 79.99m,
                    StockQuantity = 30, 
                    CategoryId = 5, 
                    SupplierId = 5,
                    SKU = "SKU-SPT-009",
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new Product 
                { 
                    ProductId = 10, 
                    ProductName = "Hiking Backpack", 
                    Description = "Durable backpack for hiking adventures", 
                    Price = 89.99m, 
                    CostPrice = 54.99m,
                    StockQuantity = 25, 
                    CategoryId = 5, 
                    SupplierId = 5,
                    SKU = "SKU-SPT-010",
                    CreatedDate = DateTime.Now.AddDays(-3)
                }
            );
        }
        
        private void SeedOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    OrderId = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now.AddDays(-10),
                    TotalAmount = 1329.98m,
                    Status = "Completed"
                },
                new Order
                {
                    OrderId = 2,
                    CustomerId = 2,
                    OrderDate = DateTime.Now.AddDays(-8),
                    TotalAmount = 259.97m,
                    Status = "Shipped"
                },
                new Order
                {
                    OrderId = 3,
                    CustomerId = 3,
                    OrderDate = DateTime.Now.AddDays(-5),
                    TotalAmount = 89.99m,
                    Status = "Processing"
                },
                new Order
                {
                    OrderId = 4,
                    CustomerId = 4,
                    OrderDate = DateTime.Now.AddDays(-3),
                    TotalAmount = 109.98m,
                    Status = "Pending"
                },
                new Order
                {
                    OrderId = 5,
                    CustomerId = 5,
                    OrderDate = DateTime.Now.AddDays(-1),
                    TotalAmount = 219.98m,
                    Status = "Processing"
                }
            );
        }
        
        private void SeedOrderDetails(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>().HasData(
                // Order 1 details
                new OrderDetail
                {
                    OrderDetailId = 1,
                    OrderId = 1,
                    ProductId = 1, // Laptop
                    Quantity = 1,
                    UnitPrice = 1299.99m,
                    TotalPrice = 1299.99m
                },
                new OrderDetail
                {
                    OrderDetailId = 2,
                    OrderId = 1,
                    ProductId = 4, // T-Shirt
                    Quantity = 1,
                    UnitPrice = 29.99m,
                    TotalPrice = 29.99m
                },
                
                // Order 2 details
                new OrderDetail
                {
                    OrderDetailId = 3,
                    OrderId = 2,
                    ProductId = 3, // Headphones
                    Quantity = 1,
                    UnitPrice = 199.99m,
                    TotalPrice = 199.99m
                },
                new OrderDetail
                {
                    OrderDetailId = 4,
                    OrderId = 2,
                    ProductId = 6, // C# Book
                    Quantity = 1,
                    UnitPrice = 49.99m,
                    TotalPrice = 49.99m
                },
                new OrderDetail
                {
                    OrderDetailId = 5,
                    OrderId = 2,
                    ProductId = 9, // Tennis Racket
                    Quantity = 1,
                    UnitPrice = 9.99m,
                    TotalPrice = 9.99m
                },
                
                // Order 3 details
                new OrderDetail
                {
                    OrderDetailId = 6,
                    OrderId = 3,
                    ProductId = 5, // Jeans
                    Quantity = 1,
                    UnitPrice = 89.99m,
                    TotalPrice = 89.99m
                },
                
                // Order 4 details
                new OrderDetail
                {
                    OrderDetailId = 7,
                    OrderId = 4,
                    ProductId = 6, // C# Book
                    Quantity = 1,
                    UnitPrice = 49.99m,
                    TotalPrice = 49.99m
                },
                new OrderDetail
                {
                    OrderDetailId = 8,
                    OrderId = 4,
                    ProductId = 7, // Database Book
                    Quantity = 1,
                    UnitPrice = 59.99m,
                    TotalPrice = 59.99m
                },
                
                // Order 5 details
                new OrderDetail
                {
                    OrderDetailId = 9,
                    OrderId = 5,
                    ProductId = 9, // Tennis Racket
                    Quantity = 1,
                    UnitPrice = 129.99m,
                    TotalPrice = 129.99m
                },
                new OrderDetail
                {
                    OrderDetailId = 10,
                    OrderId = 5,
                    ProductId = 10, // Hiking Backpack
                    Quantity = 1,
                    UnitPrice = 89.99m,
                    TotalPrice = 89.99m
                }
            );
        }
    }
}