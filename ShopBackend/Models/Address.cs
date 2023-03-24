using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName cannot be empty")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LastName cannot be empty")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email cannot be empty")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "PhoneNumber cannot be empty")]
        public int PhoneNumber { get; set; }

        [Required(ErrorMessage = "AddressLine1 cannot be empty")]
        public string? AddressLine1 { get; set; }

        [Required(ErrorMessage = "AddressLine2 cannot be empty")]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "ZipCode cannot be empty")]
        public int? ZipCode { get; set; }

        [Required(ErrorMessage = "City cannot be empty")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Country cannot be empty")]
        public string? Country { get; set; }   

        [Required(ErrorMessage = "Type cannot be empty")]
        public string? Type { get; set; }

        public virtual ICollection<Customer>? Customers { get; set; } 
    }
}
