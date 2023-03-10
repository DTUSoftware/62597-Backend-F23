
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CreateCustomerDto
    {
        public required string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Password { get; set; }

        public int Phone { get; set; }
    }
}
