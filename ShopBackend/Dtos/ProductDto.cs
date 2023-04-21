
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class ProductDto: LinkResourceBase
    {
        public required string Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string Currency { get; set; } = null!;

        public int RebateQuantity { get; set; }

        public int RebatePercent { get; set; }

        public string? UpsellProductId { get; set; }

        public string ImageUrl { get; set; } = null!;
    }
}
