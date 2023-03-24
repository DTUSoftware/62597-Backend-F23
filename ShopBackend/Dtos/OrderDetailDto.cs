
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public bool GiftWrap { get; set; }

        public bool RecurringOrder { get; set; }

        public ProductDto? Product { get; set; }
    }
}
