using System.ComponentModel.DataAnnotations;


namespace ShopBackend.Models

{
    public class Product
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public int RebateQuantity { get; set; }
        public int RebatePercent { get; set; }
        public string? UpsellProductId { get; set; }
    }
}
