using Microsoft.EntityFrameworkCore;
using RetailInventorySystem.Data;
using RetailInventorySystem.Models;

namespace RetailInventorySystem.Services
{
    public class AdvancedQueryExamples
    {
        private readonly RetailDbContext _context;
        
        public AdvancedQueryExamples(RetailDbContext context)
        {
            _context = context;
        }
        
        // Raw SQL Queries
        public async Task<IEnumerable<Product>> GetProductsWithRawSqlAsync(decimal minPrice)
        {
            return await _context.Products
                .FromSqlRaw("SELECT * FROM Products WHERE Price >= {0}", minPrice)
                .ToListAsync();
        }
        
        // Complex Join Operations
        public async Task<IEnumerable<object>> GetComplexJoinDataAsync()
        {
            return await (from p in _context.Products
                         join c in _context.Categories on p.CategoryId equals c.CategoryId
                         join s in _context.Suppliers on p.SupplierId equals s.SupplierId into supplierGroup
                         from supplier in supplierGroup.DefaultIfEmpty()
                         where p.IsActive
                         select new
                         {
                             ProductName = p.ProductName,
                             CategoryName = c.CategoryName,
                             SupplierName = supplier != null ? supplier.SupplierName : "No Supplier",
                             Price = p.Price,
                             StockQuantity = p.StockQuantity
                         }).ToListAsync();
        }
        
        // Group By Operations
        public async Task<IEnumerable<object>> GetProductsByCategory()
        {
            return await _context.Products
                .GroupBy(p => p.Category.CategoryName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count(),
                    AveragePrice = g.Average(p => p.Price),
                    TotalValue = g.Sum(p => p.Price * p.StockQuantity),
                    MinPrice = g.Min(p => p.Price),
                    MaxPrice = g.Max(p => p.Price)
                })
                .ToListAsync();
        }
        
        // Conditional Queries
        public async Task<IEnumerable<Product>> GetProductsConditionalAsync(
            string? category = null, 
            decimal? minPrice = null, 
            bool includeInactive = false)
        {
            var query = _context.Products.AsQueryable();
            
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.CategoryName == category);
            }
            
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            
            if (!includeInactive)
            {
                query = query.Where(p => p.IsActive);
            }
            
            return await query
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();
        }
        
        // Subqueries
        public async Task<IEnumerable<Product>> GetProductsWithSubqueryAsync()
        {
            var averagePrice = await _context.Products.AverageAsync(p => p.Price);
            
            return await _context.Products
                .Where(p => p.Price > averagePrice)
                .Include(p => p.Category)
                .ToListAsync();
        }
        
        // Exists Operations
        public async Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync()
        {
            return await _context.Customers
                .Where(c => _context.Orders.Any(o => o.CustomerId == c.CustomerId))
                .ToListAsync();
        }
        
        // Union Operations
        public async Task<IEnumerable<Product>> GetExpensiveOrLowStockProductsAsync()
        {
            var expensiveProducts = _context.Products.Where(p => p.Price > 500);
            var lowStockProducts = _context.Products.Where(p => p.StockQuantity < 10);
            
            return await expensiveProducts
                .Union(lowStockProducts)
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}