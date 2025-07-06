using RetailInventorySystem.Models;

namespace RetailInventorySystem.Services
{
    public interface IQueryService
    {
        // Basic Queries
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        
        // Filtering
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 20);
        
        // Sorting
        Task<IEnumerable<Product>> GetProductsSortedByPriceAsync(bool ascending = true);
        Task<IEnumerable<Product>> GetProductsSortedByNameAsync();
        Task<IEnumerable<Product>> GetRecentProductsAsync(int count = 10);
        
        // Paging
        Task<IEnumerable<Product>> GetProductsPagedAsync(int pageNumber, int pageSize);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsPagedWithCountAsync(int pageNumber, int pageSize);
        
        // Navigation Properties & Includes
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
        Task<IEnumerable<Product>> GetProductsWithSupplierAsync();
        Task<IEnumerable<Product>> GetProductsWithAllRelationsAsync();
        Task<Category?> GetCategoryWithProductsAsync(int categoryId);
        Task<Customer?> GetCustomerWithOrdersAsync(int customerId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        
        // Projections
        Task<IEnumerable<object>> GetProductSummaryAsync();
        Task<IEnumerable<object>> GetCategorySalesReportAsync();
        Task<IEnumerable<object>> GetCustomerOrderSummaryAsync();
        
        // Aggregations
        Task<decimal> GetAverageProductPriceAsync();
        Task<int> GetTotalProductsCountAsync();
        Task<decimal> GetTotalInventoryValueAsync();
        Task<object> GetProductStatisticsAsync();
        
        // Complex Queries
        Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count = 5);
        Task<IEnumerable<Customer>> GetTopCustomersByOrderValueAsync(int count = 5);
        Task<IEnumerable<object>> GetMonthlySalesReportAsync(int year);
        Task<IEnumerable<object>> GetProductPerformanceReportAsync();
        
        // Search
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Product>> SearchProductsAdvancedAsync(ProductSearchCriteria criteria);
    }
    
    public class ProductSearchCriteria
    {
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinStock { get; set; }
        public bool? IsActive { get; set; }
        public string? SKU { get; set; }
    }
}