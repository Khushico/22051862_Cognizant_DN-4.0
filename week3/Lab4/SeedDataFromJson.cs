using System.Text.Json;
using RetailInventorySystem.Data;
using RetailInventorySystem.Models;

namespace RetailInventorySystem.Services
{
    public class JsonDataSeeder
    {
        private readonly RetailDbContext _context;
        
        public JsonDataSeeder(RetailDbContext context)
        {
            _context = context;
        }
        
        public async Task SeedFromJsonAsync(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine($"JSON file not found: {jsonFilePath}");
                return;
            }
            
            var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
            var seedData = JsonSerializer.Deserialize<SeedDataModel>(jsonContent);
            
            if (seedData != null)
            {
                await SeedCategoriesFromJson(seedData.Categories);
                await SeedSuppliersFromJson(seedData.Suppliers);
                await SeedCustomersFromJson(seedData.Customers);
                await SeedProductsFromJson(seedData.Products);
            }
        }
        
        private async Task SeedCategoriesFromJson(List<Category>? categories)
        {
            if (categories != null && categories.Any())
            {
                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Seeded {categories.Count} categories from JSON");
            }
        }
        
        private async Task SeedSuppliersFromJson(List<Supplier>? suppliers)
        {
            if (suppliers != null && suppliers.Any())
            {
                await _context.Suppliers.AddRangeAsync(suppliers);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Seeded {suppliers.Count} suppliers from JSON");
            }
        }
        
        private async Task SeedCustomersFromJson(List<Customer>? customers)
        {
            if (customers != null && customers.Any())
            {
                await _context.Customers.AddRangeAsync(customers);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Seeded {customers.Count} customers from JSON");
            }
        }
        
        private async Task SeedProductsFromJson(List<Product>? products)
        {
            if (products != null && products.Any())
            {
                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Seeded {products.Count} products from JSON");
            }
        }
    }
    
    public class SeedDataModel
    {
        public List<Category> Categories { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();
        public List<Customer> Customers { get; set; } = new();
        public List<Product> Products { get; set; } = new();
    }
}