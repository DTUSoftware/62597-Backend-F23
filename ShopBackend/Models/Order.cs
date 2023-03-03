using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public String OrderStatus { get; set; } = null!;
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}
