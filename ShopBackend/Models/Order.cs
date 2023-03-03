using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Order
    {
        [Key]
        public required int Id { get; set; }
        [Required(ErrorMessage = "OrderDate cannot be empty")]
        public DateTime? OrderDate { get; set; }
        [Required(ErrorMessage = "OrderStatus cannot be empty")]
        public string? OrderStatus { get; set; }
        [Required(ErrorMessage = "CustomerId cannot be empty")]
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "OrderDetails cannot be empty")]
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
