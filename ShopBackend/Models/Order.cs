using ShopBackend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBackend.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "OrderDate cannot be empty")]
        public DateTime OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public bool CheckMarketing { get; set; }

        [Required(ErrorMessage = "Submit comment cannot be empty")]
        public string SubmitComment { get; set; } = null!;

        // Nullable for now, so that frontend can create an order without a Customer
        [Required(ErrorMessage = "CustomerEmail cannot be empty")]
        [ForeignKey("Customer")]
        public string CustomerEmail { get; set; } = null!;

        [ForeignKey("Address")]
        [Required(ErrorMessage = "ShippingAddress cannot be empty")]        
        public Guid  ShippingAddressId { get; set; }

        [ForeignKey("Address")]
        [Required(ErrorMessage = "BillingAddress cannot be empty")]
        public Guid BillingAddressId { get; set;}

        // Nullable for now, so that frontend can create an order without a Customer
        public virtual Customer? Customer { get; set; }

        public virtual Address ShippingAddress { get; set; } = null!;

        public virtual Address BillingAddress { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = null!;

        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
