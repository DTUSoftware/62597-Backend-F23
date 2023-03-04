using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ZipCode cannot be empty")]
        public int ZipCode { get; set; }

        [Required(ErrorMessage = "Country cannot be empty")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Region cannot be empty")]
        public string? Region { get; set; }

        [Required(ErrorMessage = "City cannot be empty")]
        public string? City { get; set; }

        [Required(ErrorMessage = "StreetAddress cannot be empty")]
        public string? StreetAddress { get; set; }

        [Required(ErrorMessage = "Type cannot be empty")]
        public string? Type { get; set; }
    }
}
