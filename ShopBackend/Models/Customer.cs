using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBackend.Models
{
    public class Customer
    {
        [Key]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string? Password { get; set; }

        [ForeignKey("Address")]
        public int PhysicalAddressId { get; set; }


        public virtual Address? PhysicalAddress { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
