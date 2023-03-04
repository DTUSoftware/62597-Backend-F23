
namespace ShopBackend.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public int CustomerId { get; set; }
        public ICollection<OrderDetailDto>? OrderDetails { get; set; }
    }
}
