using System.ComponentModel.DataAnnotations;


namespace ShopBackend.Models

{
    public record Product
    {
        [Key]
        public String Id { get; init; }

        [Required(ErrorMessage = "Name cannot be empty")]
        [StringLength(100)]
        public String Name { get; init; }

        [Required(ErrorMessage = "Price cannot be empty")]
        public int Price { get; init; }

        [Required(ErrorMessage = "Currency cannot be empty")]
        [StringLength(100)]
        public String Currency { get; init; }

        [Required(ErrorMessage = "RebateQuantity cannot be empty")]
        public int RebateQuantity { get; init; }

        [Required(ErrorMessage = "RebatePercent cannot be empty")]
        public int RebatePercent { get; init; }

        public String? UpsellProductId { get; init; }
    }
}
