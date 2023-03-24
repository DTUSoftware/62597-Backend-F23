using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBackend.Models
{
    public class OrderDetail
    {
        [Key]
        public required int Id { get; set; }

        [ForeignKey("Order")]
        [Required(ErrorMessage = "OrderId cannot be empty")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        [Required(ErrorMessage = "ProductId cannot be empty")]
        public string ProductId { get; set; } = null!;

        [Required(ErrorMessage = "Quantity cannot be empty")]
        public int Quantity { get; set; }

        public bool GiftWrapper { get; set; }

        public bool RecurringOrder { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
