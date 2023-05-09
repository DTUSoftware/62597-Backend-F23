namespace ShopBackend.Dtos
{
    public class UpdateUserDto
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
