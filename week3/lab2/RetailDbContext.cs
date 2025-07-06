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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure relationships
            ConfigureProductCategory(modelBuilder);
            ConfigureCustomerOrder(modelBuilder);
            ConfigureOrderOrderDetail(modelBuilder);
            ConfigureProductOrderDetail(modelBuilder);
            
            // Seed initial data
            SeedData(modelBuilder);
        }
        
        private void ConfigureProductCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        
        private void ConfigureCustomerOrder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        
        private void ConfigureOrderOrderDetail(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
        private void ConfigureProductOrderDetail(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Electronics", Description = "Electronic devices and accessories" },
                new Category { CategoryId = 2, CategoryName = "Clothing", Description = "Apparel and fashion items" },
                new Category { CategoryId = 3, CategoryName = "Books", Description = "Books and educational materials" },
                new Category { CategoryId = 4, CategoryName = "Home & Garden", Description = "Home improvement and garden supplies" }
            );
            
            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Laptop", Description = "High-performance laptop", Price = 999.99m, StockQuantity = 50, CategoryId = 1 },
                new Product { ProductId = 2, ProductName = "Smartphone", Description = "Latest smartphone", Price = 699.99m, StockQuantity = 100, CategoryId = 1 },
                new Product { ProductId = 3, ProductName = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99m, StockQuantity = 200, CategoryId = 2 },
                new Product { ProductId = 4, ProductName = "Jeans", Description = "Denim jeans", Price = 49.99m, StockQuantity = 150, CategoryId = 2 },
                new Product { ProductId = 5, ProductName = "Programming Book", Description = "Learn programming", Price = 39.99m, StockQuantity = 75, CategoryId = 3 }
            );
            
            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", Phone = "123-456-7890", Address = "123 Main St" },
                new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", Phone = "098-765-4321", Address = "456 Oak Ave" },
                new Customer { CustomerId = 3, FirstName = "Mike", LastName = "Johnson", Email = "mike.johnson@email.com", Phone = "555-123-4567", Address = "789 Pine Rd" }
            );
        }
    }
}