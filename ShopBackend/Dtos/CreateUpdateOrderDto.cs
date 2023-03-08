
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CreateUpdateOrderDto
    {
        public DateTime OrderDate { get; set; }

        public string? OrderStatus { get; set; }

        public string? CustomerEmail { get; set; }
    }
}
