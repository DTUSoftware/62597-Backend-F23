
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class OrderDto: LinkResourceBase
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public ICollection<OrderDetailDto>? OrderDetails { get; set; }
    }
}
