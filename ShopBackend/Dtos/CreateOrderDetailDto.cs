
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CreateOrderDetailDto
    {
        public int Quantity { get; set; }

        public bool GiftWrap { get; set; }

        public bool RecurringOrder { get; set; }

        public string? ProductId { get; set; }

        public Guid OrderId { get; set; }
    }
}
