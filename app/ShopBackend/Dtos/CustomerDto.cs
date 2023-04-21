
using ShopBackend.Models;
using ShopBackend.Utils;

namespace ShopBackend.Dtos
{
    public class CustomerDto
    {
        public required string Email { get; set; }

        public UserRoles Role { get; set; }

        public virtual ICollection<OrderDto>? Orders { get; set; }
    }
}
