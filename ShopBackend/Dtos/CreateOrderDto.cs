
using ShopBackend.Models;
using ShopBackend.Utils;

namespace ShopBackend.Dtos
{
    public class CreateOrderDto
    {
        public bool CheckMarketing { get; set; }

        public string SubmitComment { get; set; } = null!;

        public virtual AddressDto ShippingAddress { get; set; } = null!;

        public virtual AddressDto BillingAddress { get; set; } = null!;

        public virtual ICollection<CreateOrderDetailDto> OrderDetails { get; set; } = null!;
    }
}
