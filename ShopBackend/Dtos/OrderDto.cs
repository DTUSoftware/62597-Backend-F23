
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public Enum? OrderStatus { get; set; }

        public AddressDto? BillingAddress { get; set; }

        public AddressDto? ShippingAddress { get; set; }

        public bool CheckMarketing { get; set; }

        public string? SubmitComment { get; set; }

        public ICollection<OrderDetailDto>? OrderDetails { get; set; }
    }
}
