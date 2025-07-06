using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailInventorySystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        // New field for migration demo
        public int? SupplierId { get; set; }
        
        // New field for migration demo
        [StringLength(50)]
        public string? SKU { get; set; }
        
        // New field for migration demo
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CostPrice { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? LastUpdated { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}