
using ShopBackend.Models;

namespace ShopBackend.Dtos
//Based on https://code-maze.com/hateoas-aspnet-core-web-api/
{
    public class ProductDto: LinkResourceBase
    {
        public required string Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string? Currency { get; set; }

        public int RebateQuantity { get; set; }

        public int RebatePercent { get; set; }

        public string? UpsellProductId { get; set; }



    }
}
