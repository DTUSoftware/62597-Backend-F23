using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public int ZipCode { get; set; }
        public string Country { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string City { get; set; } = null!;
        public string StreetAddress { get; set; } = null!;
        public string Type { get; set; } = null!;

        public string CustomerEmail { get; set; } = null!;
    }
}
