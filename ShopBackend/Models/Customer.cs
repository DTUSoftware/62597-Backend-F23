using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Customer
    {
        [Key]
        public required string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Password { get; set; }

        public int Phone { get; set; }
        
        public ICollection<Address>? Address { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
