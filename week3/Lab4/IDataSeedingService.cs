namespace RetailInventorySystem.Services
{
    public interface IDataSeedingService
    {
        Task SeedAllDataAsync();
        Task SeedCategoriesAsync();
        Task SeedSuppliersAsync();
        Task SeedCustomersAsync();
        Task SeedProductsAsync();
        Task SeedOrdersAsync();
        Task<bool> HasDataAsync();
        Task ClearAllDataAsync();
    }
}