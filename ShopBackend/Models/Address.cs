using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }

        public int ZipCode { get; set; }

        public string? Country { get; set; }

        public string? Region { get; set; }

        public string? City { get; set; }

        public string? StreetAddress { get; set; }

        public string? Type { get; set; }

        public string? CustomerEmail { get; set; }

        public Customer? Customer { get; set; }
    }
}
