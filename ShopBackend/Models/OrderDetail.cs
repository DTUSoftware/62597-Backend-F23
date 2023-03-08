using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public Guid OrderId { get; set; }

        public Order? Order { get; set; }

        public string? ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
