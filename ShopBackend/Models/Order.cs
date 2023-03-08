using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string? OrderStatus { get; set; }

        public string? CustomerEmail { get; set; }

        public Customer? Customer { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
