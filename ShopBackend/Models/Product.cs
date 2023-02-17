using System.ComponentModel.DataAnnotations;


namespace ShopBackend.Models

{
    public class Product
    {
        [Key]
        public required string Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Price cannot be empty")]
        public required int Price { get; set; }

        [Required(ErrorMessage = "Currency cannot be empty")]
        [StringLength(100)]
        public required string Currency { get; set; }

        [Required(ErrorMessage = "RebateQuantity cannot be empty")]
        public required int RebateQuantity { get; set; }

        [Required(ErrorMessage = "RebatePercent cannot be empty")]
        public required int RebatePercent { get; set; }

        public string? UpsellProductId { get; set; }
    }
}
