using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RetailInventorySystem.Data;
using RetailInventorySystem.Models;

namespace RetailInventorySystem.Services
{
    public interface IQueryCachingService
    {
        Task<IEnumerable<Category>> GetCategoriesCachedAsync();
        Task<Product?> GetProductCachedAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByCategoryCachedAsync(int categoryId);
        Task InvalidateCacheAsync(string cacheKey);
        Task DemonstrateCachingPerformanceAsync();
    }
    
    public class QueryCachingService : IQueryCachingService
    {
        private readonly RetailDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _defaultCacheExpiry = TimeSpan.FromMinutes(15);
        
        public QueryCachingService(RetailDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        
        public async Task<IEnumerable<Category>> GetCategoriesCachedAsync()
        {
            const string cacheKey = "all_categories";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<Category>? cachedCategories))
            {
                Console.WriteLine("Categories loaded from cache");
                return cachedCategories!;
            }
            
            var categories = await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
            
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _defaultCacheExpiry,
                SlidingExpiration = TimeSpan.FromMinutes(5),
                Priority = CacheItemPriority.Normal
            };
            
            _cache.Set(cacheKey, categories, cacheOptions);
            Console.WriteLine("Categories loaded from database and cached");
            
            return categories;
        }
        
        public async Task<Product?> GetProductCachedAsync(int productId)
        {
            var cacheKey = $"product_{productId}";
            
            if (_cache.TryGetValue(cacheKey, out Product? cachedProduct))
            {
                Console.WriteLine($"Product {productId} loaded from cache");
                return cachedProduct;
            }
            
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
            
            if (product != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _defaultCacheExpiry,
                    Priority = CacheItemPriority.High
                };
                
                _cache.Set(cacheKey, product, cacheOptions);
                Console.WriteLine($"Product {productId} loaded from database and cached");
            }
            
            return product;
        }
        
        public async Task<IEnumerable<Product>> GetProductsByCategoryCachedAsync(int categoryId)
        {
            var cacheKey = $"products_category_{categoryId}";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<Product>? cachedProducts))
            {
                Console.WriteLine($"Products for category {categoryId} loaded from cache");
                return cachedProducts!;
            }
            
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .OrderBy(p => p.ProductName)
                .ToListAsync();
            
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(3),
                Priority = CacheItemPriority.Normal
            };
            
            _cache.Set(cacheKey, products, cacheOptions);
            Console.WriteLine($"Products for category {categoryId} loaded from database and cached");
            
            return products;
        }
        
        public Task InvalidateCacheAsync(string cacheKey)
        {
            _cache.Remove(cacheKey);
            Console.WriteLine($"Cache invalidated for key: {cacheKey}");
            return Task.CompletedTask;
        }
        
        public async Task DemonstrateCachingPerformanceAsync()
        {
            Console.WriteLine("\n=== Caching Performance Demo ===");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // First call - loads from database
            stopwatch.Restart();
            var categories1 = await GetCategoriesCachedAsync();
            stopwatch.Stop();
            Console.WriteLine($"First call (DB): {stopwatch.ElapsedMilliseconds}ms - {categories1.Count()} categories");
            
            // Second call - loads from cache
            stopwatch.Restart();
            var categories2 = await GetCategoriesCachedAsync();
            stopwatch.Stop();
            Console.WriteLine($"Second call (Cache): {stopwatch.ElapsedMilliseconds}ms - {categories2.Count()} categories");
            
            // Test product caching
            Console.WriteLine("\n--- Product Caching ---");
            
            stopwatch.Restart();
            var product1 = await GetProductCachedAsync(1);
            stopwatch.Stop();
            Console.WriteLine($"First product call (DB): {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch.Restart();
            var product2 = await GetProductCachedAsync(1);
            stopwatch.Stop();
            Console.WriteLine($"Second product call (Cache): {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}