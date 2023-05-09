using ShopBackend.Dtos;
using ShopBackend.Models;
using System.Xml.Linq;

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
                    FirstName = "Karen" ,
                    LastName = "Hansen",
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

        public static List<User> GetFakeUserList() {
            return new List<User>
            {
                new User{Email="goli@gmail.com", Password="1234", Role= Utils.UserRoles.Admin},
                new User{Email="Karen@gmail.com", Password="1234", Role= Utils.UserRoles.Customer}
            };
        }

        public static List<Order> GetFakeOrderList()
        {
            var addressList = GetFakeAddressList();
            var orderdetailList = GetFakeOrderDetails();
            return new List<Order> {
                new Order
                {
                    Id= Guid.Parse("1a1b1c1d-d9cb-469f-a165-70867728555e"),
                    OrderDate= DateTime.Now,
                    OrderStatus= Utils.OrderStatus.Finished,
                    CheckMarketing=true,
                    SubmitComment= "it is good!",
                    CustomerEmail= "janey@gmail.com",
                    ShippingAddressId= addressList[0].Id,
                    BillingAddressId= addressList[0].Id,
                    BillingAddress=addressList[0],
                    ShippingAddress=addressList[0],
                    OrderDetails = orderdetailList
                },
                new Order
                {
                    Id= Guid.Parse("2a2b2c2d-d9cb-469f-a165-70867728666e"),
                    OrderDate= DateTime.Now,
                    OrderStatus= Utils.OrderStatus.Pending,
                    CheckMarketing=false,
                    SubmitComment= "it is owsome!",
                    CustomerEmail= "bob@gmail.com",
                    ShippingAddressId= addressList[1].Id,
                    BillingAddressId= addressList[1].Id,
                    BillingAddress=addressList[1],
                    ShippingAddress=addressList[1],
                    OrderDetails = orderdetailList
                }
            };
        }

        public static List<OrderDetail> GetFakeOrderDetails()
        {
            var productList =GetFakeProducs();
            return new List<OrderDetail>
            {
                new OrderDetail
                {
                    Id = Guid.Parse("1d1d1d1d-d9db-469f-a165-70867728666e"),
                    Quantity = 2,
                    GiftWrap = true,
                    RecurringOrder = true ,
                    OrderId = Guid.Parse("1a1b1c1d-d9cb-469f-a165-70867728555e") ,
                    ProductId= productList[0].Id,
                    Product=productList[0]
                },
                new OrderDetail
                {
                    Id = Guid.Parse("2d2d2d2d-d9db-469f-a165-70867728666e"),
                    Quantity = 2,
                    GiftWrap = true,
                    RecurringOrder = true ,
                    OrderId = Guid.Parse("1a1b1c1d-d9cb-469f-a165-70867728555e") ,
                    ProductId= productList[1].Id,
                    Product=productList[1]
                }
            };
        }

        public static List<Product> GetFakeProducs()
        {
            return new List<Product>
            {
                new Product
                {
                    Id= "Orange1234",
                    Name="Orange",
                    Price= 3,
                    Currency="DKK",
                    RebateQuantity= 5,
                    RebatePercent = 10,
                    UpsellProductId="",
                    ImageUrl=""
                },
                new Product
                {
                    Id= "Apple1234",
                    Name="Orange",
                    Price= 3,
                    Currency="DKK",
                    RebateQuantity= 5,
                    RebatePercent = 10,
                    UpsellProductId="",
                    ImageUrl=""
                }
            };
        }

    }
}
