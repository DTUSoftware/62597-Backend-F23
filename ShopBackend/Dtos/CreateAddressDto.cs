
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CreateAddressDto
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? MobileNr { get; set; }

        public string? Company { get; set; }

        public string? VatNr { get; set; }

        public string? Country { get; set; }

        public string? ZipCode { get; set; }

        public string? City { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public bool IsBillingAddress { get; set; }

        public bool IsShippingAddress { get; set; }

        public Guid OrderId { get; set; }
    }
}
