
using ShopBackend.Models;
using ShopBackend.Utils;

namespace ShopBackend.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public bool CheckMarketing { get; set; }

        public string SubmitComment { get; set; } = null!;

        public virtual AddressDto ShippingAddress { get; set; } = null!;

        public virtual AddressDto BillingAddress { get; set; } = null!;

        public virtual ICollection<OrderDetailDto> OrderDetails { get; set; } = null!;
    }
}
