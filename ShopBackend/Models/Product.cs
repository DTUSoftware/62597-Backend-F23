using System.ComponentModel.DataAnnotations;


namespace ShopBackend.Models

{
    public class Product
    {
        [Key]
        public required string Id { get; set; }
        
        [Required(ErrorMessage = "Name cannot be empty")]
        [StringLength(100)]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "Price cannot be empty")]
        public decimal? Price { get; set; }
        
        [Required(ErrorMessage = "Currency cannot be empty")]
        [StringLength(100)]
        public string? Currency { get; set; }
        
        [Required(ErrorMessage = "RebateQuantity cannot be empty")]
        public int? RebateQuantity { get; set; }
        
        [Required(ErrorMessage = "RebatePercent cannot be empty")]
        public int? RebatePercent { get; set; }
        [StringLength(100)]
        public string? UpsellProductId { get; set; }

        public string? ImageUrl { get; set; }
    }
}
