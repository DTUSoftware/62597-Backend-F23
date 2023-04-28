using ShopBackend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBackend.Models
{
    public class Customer
    {
        [Key]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; } = null!;

        public UserRoles Role { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }

        [Timestamp]
        public byte[]? Version { get; set; }
    }
}

