namespace ShopBackend.Dtos
{
    public class UpdateCustomerDto
    {
        public required string Email { get; set; }

        public string Password { get; set; } = null!;

        public virtual ICollection<OrderDto>? Orders { get; set; }
    }
}
