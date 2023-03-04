using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Quantity cannot be empty")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Product cannot be empty")]
        public Product? Product { get; set; }
    }
}
