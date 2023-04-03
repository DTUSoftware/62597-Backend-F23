using ShopBackend.Models;

namespace ShopBackend.Test
{
    public class DataHelper
    {
        public static List<Address> GetFakeAddressList()
        {
            return new List<Address>
            {
                new Address{
                    Id= Guid.Parse("1a1b1c1d-d9cb-469f-a165-70867728950e"),
                    FirstName = "Goli" ,
                    LastName = "Haidari",
                    Email="goli@gmail.com",
                    MobileNr = "12345678",
                    Company = "DTU",
                    VatNr = "12345678",
                    Country = "Denmark",
                    ZipCode = "1900",
                        City = "Frederiksberg C",
                    Address1 = "Gammel kongvej 58",
                    Address2 = "2 sal, tv"
                },
                new Address{
                    Id= Guid.Parse("2a2b2c2d-d9cb-469f-a165-70867728950e"),
                    FirstName = "Goli" ,
                    LastName = "Haidari",
                    Email="Karen@gmail.com",
                    MobileNr = "12345678",
                    Company = "DTU",
                    VatNr = "12345678",
                    Country = "Denmark",
                    ZipCode = "1300",
                    City = "København",
                    Address1 = "Gammel kongvej 120",
                    Address2 = "st, tv"
                }
            };
        }

        public static List<Customer> GetFakeCustomerList() {
            return new List<Customer>
            {
                new Customer{Email="goli@gmail.com", Password="1234"},
                new Customer{Email="Karen@gmail.com", Password="1234"}
            };
        }
    }
}
