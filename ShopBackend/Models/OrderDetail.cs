using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBackend.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public bool GiftWrap { get; set; }

        public bool RecurringOrder { get; set; }


        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; } = null!;

        [ForeignKey("Product")]
        public string ProductId { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;

        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
