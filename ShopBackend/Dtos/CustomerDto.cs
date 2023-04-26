
using ShopBackend.Discoverabillity;
using ShopBackend.Utils;

namespace ShopBackend.Dtos
{
    public class CustomerDto: LinkResourceBase
    {
        public required string Email { get; set; }

        public UserRoles Role { get; set; }

        public virtual ICollection<OrderDto>? Orders { get; set; }
    }
}
