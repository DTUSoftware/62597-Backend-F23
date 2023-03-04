using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Customer
    {
        [Key]
        public required string Email { get; set; }

        [Required(ErrorMessage = "FirstName cannot be empty")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName cannot be empty")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Phone cannot be empty")]
        public int Phone { get; set; }
        
        [Required(ErrorMessage = "Address cannot be empty")]
        public ICollection<Address>? Address { get; set; }

        [Required(ErrorMessage = "Orders cannot be empty")]
        public ICollection<Order>? Orders { get; set; }
    }
}
