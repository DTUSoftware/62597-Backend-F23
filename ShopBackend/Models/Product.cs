using ShopBackend.Dtos;
using System.ComponentModel.DataAnnotations;


namespace ShopBackend.Models

{
    public class Product
    {
        [Key]
        public required string Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string? Currency { get; set; }

        public int RebateQuantity { get; set; }

        public int RebatePercent { get; set; }

        public string? UpsellProductId { get; set; }

        public string? ImageUrl { get; set; }



        public ICollection<OrderDetail>? OrderDetails { get; set; }

        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
