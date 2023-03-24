using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


        [Required(ErrorMessage = "CustomerEmail cannot be empty")]
        [ForeignKey("Customer")]
        public string Email { get; set; } = null!;

        [ForeignKey("Address")]
        [Required(ErrorMessage = "ShippingAddress cannot be empty")]        
        public int  ShippingAddressId { get; set; }


        [ForeignKey("Address")]
        [Required(ErrorMessage = "BillingAddress cannot be empty")]
        public int BillingAddressId { get; set;}
                

       
        public virtual Customer Customer { get; set; } = null!;
        public virtual Address ShippingAddress { get; set; } = null!;
        public virtual Address BillingAddress { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = null!;

    }
}
