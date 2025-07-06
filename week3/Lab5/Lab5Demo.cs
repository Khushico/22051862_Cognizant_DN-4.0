using Microsoft.Extensions.DependencyInjection;
using RetailInventorySystem.Services;

namespace RetailInventorySystem
{
    public class Lab5Demo
    {
        public static async Task RunQueryDemonstrations(IServiceProvider serviceProvider)
        {
            Console.WriteLine("\n=== LAB 5: Retrieving Data Demo ===");
            
            var queryService = serviceProvider.GetRequiredService<IQueryService>();
            var advancedQueries = serviceProvider.GetRequiredService<AdvancedQueryExamples>();
            
            await DemonstrateBasicQueries(queryService);
            await DemonstrateFiltering(queryService);
            await DemonstrateSorting(queryService);
            await DemonstratePaging(queryService);
            await DemonstrateIncludes(queryService);
            await DemonstrateProjections(queryService);
            await DemonstrateAggregations(queryService);
            await DemonstrateComplexQueries(queryService);
            await DemonstrateSearch(queryService);
            await DemonstrateAdvancedQueries(advancedQueries);
        }
        
        private static async Task DemonstrateBasicQueries(IQueryService queryService)
        {
            Console.WriteLine("\n--- Basic Queries ---");
            
            // Get all products
            var allProducts = await queryService.GetAllProductsAsync();
            Console.WriteLine($"Total products: {allProducts.Count()}");
            
            // Get product by ID
            var product = await queryService.GetProductByIdAsync(1);
            Console.WriteLine($"Product by ID 1: {product?.ProductName ?? "Not found"}");
            
            // Get products by name
            var laptops = await queryService.GetProductsByNameAsync("Laptop");
            Console.WriteLine($"Products containing 'Laptop': {laptops.Count()}");
        }
        
        private static async Task DemonstrateFiltering(IQueryService queryService)
        {
            Console.WriteLine("\n--- Filtering ---");
            
            // Price range filtering
            var expensiveProducts = await queryService.GetProductsByPriceRangeAsync(100, 1000);
            Console.WriteLine($"Products in $100-$1000 range: {expensiveProducts.Count()}");
            
            // Category filtering
            var electronics = await queryService.GetProductsByCategoryAsync("Electronics");
            Console.WriteLine($"Electronics products: {electronics.Count()}");
            
            // Low stock products
            var lowStock = await queryService.GetLowStockProductsAsync(30);
            Console.WriteLine($"Low stock products (≤30): {lowStock.Count()}");
            foreach (var p in lowStock.Take(3))
            {
                Console.WriteLine($"  • {p.ProductName}: {p.StockQuantity} units");
            }
        }
        
        private static async Task DemonstrateSorting(IQueryService queryService)
        {
            Console.WriteLine("\n--- Sorting ---");
            
            // Sort by price
            var sortedByPrice = await queryService.GetProductsSortedByPriceAsync(false);
            Console.WriteLine("Top 3 most expensive products:");
            foreach (var p in sortedByPrice.Take(3))
            {
                Console.WriteLine($"  • {p.ProductName}: ${p.Price:F2}");
            }
            
            // Recent products
            var recentProducts = await queryService.GetRecentProductsAsync(3);
            Console.WriteLine("\nMost recent products:");
            foreach (var p in recentProducts)
            {
                Console.WriteLine($"  • {p.ProductName} (Created: {p.CreatedDate:yyyy-MM-dd})");
            }
        }
        
        private static async Task DemonstratePaging(IQueryService queryService)
        {
            Console.WriteLine("\n--- Paging ---");
            
            // Simple paging
            var pagedProducts = await queryService.GetProductsPagedAsync(1, 3);
            Console.WriteLine("Page 1 (3 products):");
            foreach (var p in pagedProducts)
            {
                Console.WriteLine($"  • {p.ProductName}");
            }
            
            // Paging with count
            var (products, totalCount) = await queryService.GetProductsPagedWithCountAsync(1, 5);
            Console.WriteLine($"\nPage 1 of products (5 per page, {totalCount} total):");
            foreach (var p in products)
            {
                Console.WriteLine($"  • {p.ProductName}");
            }
        }
        
        private static async Task DemonstrateIncludes(IQueryService queryService)
        {
            Console.WriteLine("\n--- Navigation Properties & Includes ---");
            
            // Products with category
            var productsWithCategory = await queryService.GetProductsWithCategoryAsync();
            Console.WriteLine("Products with categories:");
            foreach (var p in productsWithCategory.Take(3))
            {
                Console.WriteLine($"  • {p.ProductName} - Category: {p.Category.CategoryName}");
            }
            
            // Category with products
            var categoryWithProducts = await queryService.GetCategoryWithProductsAsync(1);
            if (categoryWithProducts != null)
            {
                Console.WriteLine($"\nCategory '{categoryWithProducts.CategoryName}' has {categoryWithProducts.Products.Count} products");
            }
            
            // Customer with orders
            var customerWithOrders = await queryService.GetCustomerWithOrdersAsync(1);
            if (customerWithOrders != null)
            {
                Console.WriteLine($"\nCustomer '{customerWithOrders.FirstName} {customerWithOrders.LastName}' has {customerWithOrders.Orders.Count} orders");
            }
        }
        
        private static async Task DemonstrateProjections(IQueryService queryService)
        {
            Console.WriteLine("\n--- Projections ---");
            
            // Product summary projection
            var productSummary = await queryService.GetProductSummaryAsync();
            Console.WriteLine("Product Summary (first 3):");
            foreach (var item in productSummary.Take(3))
            {
                Console.WriteLine($"  • {item}");
            }
            
            // Category sales report
            var categorySales = await queryService.GetCategorySalesReportAsync();
            Console.WriteLine("\nCategory Sales Report:");
            foreach (var item in categorySales.Take(3))
            {
                Console.WriteLine($"  • {item}");
            }
        }
        
        private static async Task DemonstrateAggregations(IQueryService queryService)
        {
            Console.WriteLine("\n--- Aggregations ---");
            
            var avgPrice = await queryService.GetAverageProductPriceAsync();
            var totalProducts = await queryService.GetTotalProductsCountAsync();
            var totalValue = await queryService.GetTotalInventoryValueAsync();
            var stats = await queryService.GetProductStatisticsAsync();
            
            Console.WriteLine($"Average Product Price: ${avgPrice:F2}");
            Console.WriteLine($"Total Active Products: {totalProducts}");
            Console.WriteLine($"Total Inventory Value: ${totalValue:F2}");
            Console.WriteLine($"Product Statistics: {stats}");
        }
        
        private static async Task DemonstrateComplexQueries(IQueryService queryService)
        {
            Console.WriteLine("\n--- Complex Queries ---");
            
            // Top selling products
            var topSelling = await queryService.GetTopSellingProductsAsync(3);
            Console.WriteLine("Top 3 selling products:");
            foreach (var p in topSelling)
            {
                var totalSold = p.OrderDetails.Sum(od => od.Quantity);
                Console.WriteLine($"  • {p.ProductName}: {totalSold} units sold");
            }
            
            // Top customers
            var topCustomers = await queryService.GetTopCustomersByOrderValueAsync(3);
            Console.WriteLine("\nTop 3 customers by order value:");
            foreach (var c in topCustomers)
            {
                var totalSpent = c.Orders.Sum(o => o.TotalAmount);
                Console.WriteLine($"  • {c.FirstName} {c.LastName}: ${totalSpent:F2}");
            }
            
            // Monthly sales report
            var monthlySales = await queryService.GetMonthlySalesReportAsync(DateTime.Now.Year);
            Console.WriteLine($"\nMonthly Sales Report for {DateTime.Now.Year}:");
            foreach (var month in monthlySales.Take(3))
            {
                Console.WriteLine($"  • {month}");
            }
        }
        
        private static async Task DemonstrateSearch(IQueryService queryService)
        {
            Console.WriteLine("\n--- Search ---");
            
            // Simple search
            var searchResults = await queryService.SearchProductsAsync("Pro");
            Console.WriteLine($"Products containing 'Pro': {searchResults.Count()}");
            foreach (var p in searchResults.Take(3))
            {
                Console.WriteLine($"  • {p.ProductName}");
            }
            
            // Advanced search
            var searchCriteria = new ProductSearchCriteria
            {
                CategoryName = "Electronics",
                MinPrice = 100,
                MaxPrice = 2000,
                IsActive = true
            };
            
            var advancedResults = await queryService.SearchProductsAdvancedAsync(searchCriteria);
            Console.WriteLine($"\nAdvanced search results: {advancedResults.Count()}");
            foreach (var p in advancedResults.Take(3))
            {
                Console.WriteLine($"  • {p.ProductName} - ${p.Price:F2}");
            }
        }
        
        private static async Task DemonstrateAdvancedQueries(AdvancedQueryExamples advancedQueries)
        {
            Console.WriteLine("\n--- Advanced Query Techniques ---");
            
            try
            {
                // Complex join operations
                var joinData = await advancedQueries.GetComplexJoinDataAsync();
                Console.WriteLine($"Complex join results: {joinData.Count()}");
                foreach (var item in joinData.Take(3))
                {
                    Console.WriteLine($"  • {item}");
                }
                
                // Group by operations
                var groupedData = await advancedQueries.GetProductsByCategory();
                Console.WriteLine("\nProducts grouped by category:");
                foreach (var group in groupedData.Take(3))
                {
                    Console.WriteLine($"  • {group}");
                }
                
                // Conditional queries
                var conditionalResults = await advancedQueries.GetProductsConditionalAsync(
                    category: "Electronics", 
                    minPrice: 200, 
                    includeInactive: false);
                Console.WriteLine($"\nConditional query results: {conditionalResults.Count()}");
                
                // Subqueries
                var subqueryResults = await advancedQueries.GetProductsWithSubqueryAsync();
                Console.WriteLine($"Products above average price: {subqueryResults.Count()}");
                
                // Union operations
                var unionResults = await advancedQueries.GetExpensiveOrLowStockProductsAsync();
                Console.WriteLine($"Expensive or low stock products: {unionResults.Count()}");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Advanced query demonstration error: {ex.Message}");
            }
        }
    }
}