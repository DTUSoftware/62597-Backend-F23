
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CreateOrderDto
    {
        public ICollection<CreateOrderDetailDto>? OrderDetails { get; set; }

        public CreateAddressDto? BillingAddress { get; set; }

        public CreateAddressDto? ShippingAddress { get; set; }

        public bool CheckMarketing { get; set; }

        public string? SubmitComment { get; set; }

        public string? CustomerEmail { get; set; }
    }
}
