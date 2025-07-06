using Microsoft.EntityFrameworkCore;
using RetailInventorySystem.Data;
using RetailInventorySystem.Models;

namespace RetailInventorySystem.Services
{
    public interface IQueryOptimizationService
    {
        Task DemonstrateQueryOptimizationAsync();
    }
    
    public class QueryOptimizationService : IQueryOptimizationService
    {
        private readonly RetailDbContext _context;
        
        public QueryOptimizationService(RetailDbContext context)
        {
            _context = context;
        }
        
        public async Task DemonstrateQueryOptimizationAsync()
        {
            Console.WriteLine("\n=== Query Optimization Techniques ===");
            
            await DemonstrateAsNoTracking();
            await DemonstrateSelectiveProjection();
            await DemonstrateBatchLoading();
            await DemonstrateQuerySplitting();
            await DemonstrateCompiledQueries();
        }
        
        private async Task DemonstrateAsNoTracking()
        {
            Console.WriteLine("\n--- AsNoTracking Performance ---");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Without AsNoTracking (tracked entities)
            stopwatch.Restart();
            var trackedProducts = await _context.Products.Take(100).ToListAsync();
            stopwatch.Stop();
            Console.WriteLine($"Tracked query time: {stopwatch.ElapsedMilliseconds}ms");
            
            // With AsNoTracking (better performance for read-only)
            stopwatch.Restart();
            var untrackedProducts = await _context.Products.AsNoTracking().Take(100).ToListAsync();
            stopwatch.Stop();
            Console.WriteLine($"AsNoTracking query time: {stopwatch.ElapsedMilliseconds}ms");
        }
        
        private async Task DemonstrateSelectiveProjection()
        {
            Console.WriteLine("\n--- Selective Projection Performance ---");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Loading full entities
            stopwatch.Restart();
            var fullEntities = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Take(50)
                .ToListAsync();
            stopwatch.Stop();
            Console.WriteLine($"Full entity load time: {stopwatch.ElapsedMilliseconds}ms");
            
            // Selective projection (only needed fields)
            stopwatch.Restart();
            var projectedData = await _context.Products
                .Select(p => new
                {
                    p.ProductName,
                    p.Price,
                    CategoryName = p.Category.CategoryName,
                    SupplierName = p.Supplier != null ? p.Supplier.SupplierName : null
                })
                .Take(50)
                .ToListAsync();
            stopwatch.Stop();
            Console.WriteLine($"Projected data load time: {stopwatch.ElapsedMilliseconds}ms");
        }
        
        private async Task DemonstrateBatchLoading()
        {
            Console.WriteLine("\n--- Batch Loading vs N+1 Problem ---");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // N+1 Problem (bad practice)
            stopwatch.Restart();
            var productsN1 = await _context.Products.Take(10).ToListAsync();
            foreach (var product in productsN1)
            {
                // This causes N+1 queries
                var category = await _context.Categories.FindAsync(product.CategoryId);
            }
            stopwatch.Stop();
            Console.WriteLine($"N+1 query time: {stopwatch.ElapsedMilliseconds}ms");
            
            // Batch loading with Include (good practice)
            stopwatch.Restart();
            var productsBatch = await _context.Products
                .Include(p => p.Category)
                .Take(10)
                .ToListAsync();
            stopwatch.Stop();
            Console.WriteLine($"Batch loading time: {stopwatch.ElapsedMilliseconds}ms");
        }
        
        private async Task DemonstrateQuerySplitting()
        {
            Console.WriteLine("\n--- Query Splitting ---");
            
            try
            {
                // Single query (can cause cartesian explosion)
                var singleQuery = await _context.Categories
                    .Include(c => c.Products)
                        .ThenInclude(p => p.OrderDetails)
                    .Take(3)
                    .ToListAsync();
                Console.WriteLine($"Single query loaded {singleQuery.Count} categories");
                
                // Split query (multiple queries, better performance)
                var splitQuery = await _context.Categories
                    .AsSplitQuery()
                    .Include(c => c.Products)
                        .ThenInclude(p => p.OrderDetails)
                    .Take(3)
                    .ToListAsync();
                Console.WriteLine($"Split query loaded {splitQuery.Count} categories");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Query splitting demo error: {ex.Message}");
            }
        }
        
        private async Task DemonstrateCompiledQueries()
        {
            Console.WriteLine("\n--- Compiled Queries ---");
            
            // Define compiled query (better performance for repeated queries)
            var compiledQuery = EF.CompileAsyncQuery((RetailDbContext context, decimal minPrice) =>
                context.Products
                    .AsNoTracking()
                    .Where(p => p.Price >= minPrice)
                    .Select(p => new { p.ProductName, p.Price }));
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // First execution
            stopwatch.Restart();
            var result1 = await compiledQuery(_context, 100);
            stopwatch.Stop();
            Console.WriteLine($"First compiled query execution: {stopwatch.ElapsedMilliseconds}ms");
            
            // Subsequent executions (should be faster)
            stopwatch.Restart();
            var result2 = await compiledQuery(_context, 200);
            stopwatch.Stop();
            Console.WriteLine($"Second compiled query execution: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}