using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBackend.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Quantity cannot be empty")]
        public int Quantity { get; set; }

        public bool GiftWrap { get; set; }

        public bool RecurringOrder { get; set; }

        [Required(ErrorMessage = "OrderId cannot be empty")]
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "ProductId cannot be empty")]
        public string ProductId { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
