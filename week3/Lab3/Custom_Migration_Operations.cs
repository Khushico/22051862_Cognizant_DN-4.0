// Create a custom migration file: Migrations/20240101000000_CustomDataMigration.cs
using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailInventorySystem.Migrations
{
    public partial class CustomDataMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add custom logic for data transformation
            migrationBuilder.Sql(@"
                UPDATE Products 
                SET SKU = 'SKU-' + CAST(ProductId AS VARCHAR(10))
                WHERE SKU IS NULL
            ");
            
            migrationBuilder.Sql(@"
                UPDATE Products 
                SET CostPrice = Price * 0.6
                WHERE CostPrice IS NULL
            ");
        }
        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback logic
            migrationBuilder.Sql(@"
                UPDATE Products 
                SET SKU = NULL, CostPrice = NULL
            ");
        }
    }
}