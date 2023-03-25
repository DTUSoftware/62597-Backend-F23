
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CustomerDto
    {
        public required string Email { get; set; }

        public virtual ICollection<OrderDto>? Orders { get; set; }
    }
}
