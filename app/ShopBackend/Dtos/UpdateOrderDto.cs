using ShopBackend.Utils;

namespace ShopBackend.Dtos
{
    public class UpdateOrderDto
    {
        public Guid Id { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public bool CheckMarketing { get; set; }

        public string SubmitComment { get; set; } = null!;

        // Nullable for now, so that frontend can create an order without a Customer
        public string? CustomerEmail { get; set; }
    }
}
