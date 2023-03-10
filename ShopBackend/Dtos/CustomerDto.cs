
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CustomerDto
    {
        public required string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int Phone { get; set; }

        public ICollection<AddressDto>? Address { get; set; }

        public ICollection<OrderDto>? Orders { get; set; }
    }
}
