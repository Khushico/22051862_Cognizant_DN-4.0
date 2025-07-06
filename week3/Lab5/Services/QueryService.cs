using Microsoft.EntityFrameworkCore;
using RetailInventorySystem.Data;
using RetailInventorySystem.Models;

namespace RetailInventorySystem.Services
{
    public class QueryService : IQueryService
    {
        private readonly RetailDbContext _context;
        
        public QueryService(RetailDbContext context)
        {
            _context = context;
        }
        
        #region Basic Queries
        
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        
        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.ProductName.Contains(name))
                .ToListAsync();
        }
        
        #endregion
        
        #region Filtering
        
        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.Category.CategoryName == categoryName)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 20)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity <= threshold && p.IsActive)
                .OrderBy(p => p.StockQuantity)
                .ToListAsync();
        }
        
        #endregion
        
        #region Sorting
        
        public async Task<IEnumerable<Product>> GetProductsSortedByPriceAsync(bool ascending = true)
        {
            var query = _context.Products.AsNoTracking();
            
            return ascending 
                ? await query.OrderBy(p => p.Price).ToListAsync()
                : await query.OrderByDescending(p => p.Price).ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetProductsSortedByNameAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetRecentProductsAsync(int count = 10)
        {
            return await _context.Products
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedDate)
                .Take(count)
                .ToListAsync();
        }
        
        #endregion
        
        #region Paging
        
        public async Task<IEnumerable<Product>> GetProductsPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        
        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsPagedWithCountAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Products.CountAsync();
            
            var products = await _context.Products
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return (products, totalCount);
        }
        
        #endregion
        
        #region Navigation Properties & Includes
        
        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetProductsWithSupplierAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Supplier)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetProductsWithAllRelationsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.OrderDetails)
                    .ThenInclude(od => od.Order)
                .ToListAsync();
        }
        
        public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
        
        public async Task<Customer?> GetCustomerWithOrdersAsync(int customerId)
        {
            return await _context.Customers
                .AsNoTracking()
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
        
        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
        
        #endregion
        
        #region Projections
        
        public async Task<IEnumerable<object>> GetProductSummaryAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.Price,
                    p.StockQuantity,
                    CategoryName = p.Category.CategoryName,
                    SupplierName = p.Supplier != null ? p.Supplier.SupplierName : "No Supplier",
                    InventoryValue = p.Price * p.StockQuantity
                })
                .ToListAsync();
        }
        
        public async Task<IEnumerable<object>> GetCategorySalesReportAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Select(c => new
                {
                    CategoryName = c.CategoryName,
                    ProductCount = c.Products.Count,
                    TotalInventoryValue = c.Products.Sum(p => p.Price * p.StockQuantity),
                    AveragePrice = c.Products.Average(p => p.Price),
                    TotalSold = c.Products
                        .SelectMany(p => p.OrderDetails)
                        .Sum(od => od.Quantity)
                })
                .ToListAsync();
        }
        
        public async Task<IEnumerable<object>> GetCustomerOrderSummaryAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .Select(c => new
                {
                    CustomerName = c.FirstName + " " + c.LastName,
                    c.Email,
                    OrderCount = c.Orders.Count,
                    TotalSpent = c.Orders.Sum(o => o.TotalAmount),
                    LastOrderDate = c.Orders.Max(o => o.OrderDate),
                    AverageOrderValue = c.Orders.Average(o => o.TotalAmount)
                })
                .ToListAsync();
        }
        
        #endregion
        
        #region Aggregations
        
        public async Task<decimal> GetAverageProductPriceAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .AverageAsync(p => p.Price);
        }
        
        public async Task<int> GetTotalProductsCountAsync()
        {
            return await _context.Products
                .CountAsync(p => p.IsActive);
        }
        
        public async Task<decimal> GetTotalInventoryValueAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .SumAsync(p => p.Price * p.StockQuantity);
        }
        
        public async Task<object> GetProductStatisticsAsync()
        {
            var stats = await _context.Products
                .Where(p => p.IsActive)
                .GroupBy(p => 1)
                .Select(g => new
                {
                    TotalProducts = g.Count(),
                    AveragePrice = g.Average(p => p.Price),
                    MinPrice = g.Min(p => p.Price),
                    MaxPrice = g.Max(p => p.Price),
                    TotalInventoryValue = g.Sum(p => p.Price * p.StockQuantity),
                    TotalStockQuantity = g.Sum(p => p.StockQuantity)
                })
                .FirstOrDefaultAsync();
            
            return stats ?? new { };
        }
        
        #endregion
        
        #region Complex Queries
        
        public async Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count = 5)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.OrderDetails)
                .Where(p => p.OrderDetails.Any())
                .OrderByDescending(p => p.OrderDetails.Sum(od => od.Quantity))
                .Take(count)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Customer>> GetTopCustomersByOrderValueAsync(int count = 5)
        {
            return await _context.Customers
                .AsNoTracking()
                .Include(c => c.Orders)
                .Where(c => c.Orders.Any())
                .OrderByDescending(c => c.Orders.Sum(o => o.TotalAmount))
                .Take(count)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<object>> GetMonthlySalesReportAsync(int year)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Year == year)
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    MonthName = new DateTime(year, g.Key, 1).ToString("MMMM"),
                    OrderCount = g.Count(),
                    TotalSales = g.Sum(o => o.TotalAmount),
                    AverageOrderValue = g.Average(o => o.TotalAmount)
                })
                .OrderBy(x => x.Month)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<object>> GetProductPerformanceReportAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.OrderDetails)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    p.Price,
                    p.StockQuantity,
                    TotalSold = p.OrderDetails.Sum(od => od.Quantity),
                    Revenue = p.OrderDetails.Sum(od => od.TotalPrice),
                    ProfitMargin = p.CostPrice.HasValue ? ((p.Price - p.CostPrice.Value) / p.Price) * 100 : 0,
                    LastSold = p.OrderDetails.Max(od => od.Order.OrderDate)
                })
                .OrderByDescending(x => x.Revenue)
                .ToListAsync();
        }
        
        #endregion
        
        #region Search
        
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Where(p => p.ProductName.Contains(searchTerm) ||
                           p.Description.Contains(searchTerm) ||
                           p.SKU!.Contains(searchTerm) ||
                           p.Category.CategoryName.Contains(searchTerm))
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> SearchProductsAdvancedAsync(ProductSearchCriteria criteria)
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();
            
            if (!string.IsNullOrEmpty(criteria.ProductName))
            {
                query = query.Where(p => p.ProductName.Contains(criteria.ProductName));
            }
            
            if (!string.IsNullOrEmpty(criteria.CategoryName))
            {
                query = query.Where(p => p.Category.CategoryName.Contains(criteria.CategoryName));
            }
            
            if (!string.IsNullOrEmpty(criteria.SupplierName))
            {
                query = query.Where(p => p.Supplier != null && p.Supplier.SupplierName.Contains(criteria.SupplierName));
            }
            
            if (criteria.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= criteria.MinPrice.Value);
            }
            
            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= criteria.MaxPrice.Value);
            }
            
            if (criteria.MinStock.HasValue)
            {
                query = query.Where(p => p.StockQuantity >= criteria.MinStock.Value);
            }
            
            if (criteria.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == criteria.IsActive.Value);
            }
            
            if (!string.IsNullOrEmpty(criteria.SKU))
            {
                query = query.Where(p => p.SKU!.Contains(criteria.SKU));
            }
            
            return await query.ToListAsync();
        }
        
        #endregion
    }
}