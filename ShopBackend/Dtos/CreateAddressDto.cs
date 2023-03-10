
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CreateAddressDto
    {
        public int ZipCode { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? StreetAddress { get; set; }

        public string? Type { get; set; }

        public string? CustomerEmail { get; set; }
    }
}
