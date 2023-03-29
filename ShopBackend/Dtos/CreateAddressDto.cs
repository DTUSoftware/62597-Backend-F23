
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CreateAddressDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string MobileNr { get; set; } = null!;

        public string Company { get; set; } = null!;

        public string VatNr { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Address1 { get; set; } = null!;

        public string Address2 { get; set; } = null!;
    }
}
