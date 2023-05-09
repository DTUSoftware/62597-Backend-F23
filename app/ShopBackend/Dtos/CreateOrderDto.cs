
using ShopBackend.Models;
using ShopBackend.Utils;

namespace ShopBackend.Dtos
{
    public class CreateOrderDto
    {
        public Guid Id { get; set; }
        public bool CheckMarketing { get; set; }

        public string SubmitComment { get; set; } = null!;

        // Nullable for now, so that frontend can create an order without a Customer
        public string? CustomerEmail { get; set; }

        public virtual CreateAddressDto ShippingAddress { get; set; } = null!;

        public virtual CreateAddressDto BillingAddress { get; set; } = null!;

        public virtual ICollection<CreateOrderDetailDto> OrderDetails { get; set; } = null!;
    }
}
