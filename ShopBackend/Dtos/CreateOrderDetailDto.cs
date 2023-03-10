
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CreateOrderDetailDto
    {
        public int Quantity { get; set; }

        public Guid OrderId { get; set; }

        public string? ProductId { get; set; }
    }
}
