using Microsoft.EntityFrameworkCore;
using RetailInventorySystem.Data;

namespace RetailInventorySystem.Services
{
    public interface IMigrationService
    {
        Task<bool> IsDatabaseUpToDateAsync();
        Task<IEnumerable<string>> GetPendingMigrationsAsync();
        Task<IEnumerable<string>> GetAppliedMigrationsAsync();
        Task ApplyMigrationsAsync();
        Task<string> GenerateMigrationScriptAsync();
    }
    
    public class MigrationService : IMigrationService
    {
        private readonly RetailDbContext _context;
        
        public MigrationService(RetailDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> IsDatabaseUpToDateAsync()
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            return !pendingMigrations.Any();
        }
        
        public async Task<IEnumerable<string>> GetPendingMigrationsAsync()
        {
            return await _context.Database.GetPendingMigrationsAsync();
        }
        
        public async Task<IEnumerable<string>> GetAppliedMigrationsAsync()
        {
            return await _context.Database.GetAppliedMigrationsAsync();
        }
        
        public async Task ApplyMigrationsAsync()
        {
            await _context.Database.MigrateAsync();
        }
        
        public async Task<string> GenerateMigrationScriptAsync()
        {
            return await _context.Database.GenerateCreateScriptAsync();
        }
    }
}