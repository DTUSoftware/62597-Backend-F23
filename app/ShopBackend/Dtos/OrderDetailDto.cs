
using ShopBackend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBackend.Dtos
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public bool GiftWrap { get; set; }

        public bool RecurringOrder { get; set; }

        public virtual OrderDto Order { get; set; } = null!;

        public virtual ProductDto Product { get; set; } = null!;
    }
}
