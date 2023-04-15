namespace ShopBackend.Dtos
{
    public class UpdateCustomerDto
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public virtual ICollection<OrderDto>? Orders { get; set; }
    }
}
