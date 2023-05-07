using Moq;
using ShopBackend.Models;
using ShopBackend.Dtos;
using ShopBackend.Repositories;
using ShopBackend.Security;
using System.Security.Claims;

namespace ShopBackend.Test
{
    public class MockIRepositories
    {
        public static Mock<IAuthService> GetAuthService(List<User> userList)
        {
            var mock = new Mock<IAuthService>();

            mock.Setup(ar => ar.AuthenticateUser(It.IsAny<LoginDto>())).Returns((LoginDto user) =>
            {
                if (userList.Exists(u => u.Email == user.Email)) { return Task.FromResult(true); }

                else { return Task.FromResult(false); }
            });

            mock.Setup(ar => ar.CreateToken()).Returns("123456789");
            mock.Setup(ar => ar.GetRoleFromToken(It.IsAny<ClaimsPrincipal>())).Returns("Admin");
            mock.Setup(ar => ar.GetEmailFromToken(It.IsAny<ClaimsPrincipal>())).Returns("goli@gmail.com");

            return mock;
        }

        public static Mock<IUserRepository> GetUserRepository(List<User> userList)
        {
            var mock = new Mock<IUserRepository>();

            mock.Setup(urm => urm.GetAll()).ReturnsAsync(() => userList);

            mock.Setup(urm => urm.Get(It.IsAny<string>())).Returns((string email) =>
            {
                User? user = userList.FirstOrDefault(u => u.Email == email);
                return Task.FromResult(user);
            });

            mock.Setup(urm => urm.Insert(It.IsAny<User>())).Returns((User newUser) =>
            {
                if (newUser.Email == null || userList.Exists(u => u.Email == newUser.Email)) { return Task.FromResult(0); }

                else { userList.Add(newUser); return Task.FromResult(1); }
            });

            mock.Setup(crm => crm.Update(It.IsAny<User>())).Returns((User targetUser) =>
            {
                if (targetUser.Email == null || !userList.Exists(u => u.Email == targetUser.Email)) { return Task.FromResult(0); }
                else
                {
                    var orginal = userList.Where(u => u.Email == targetUser.Email).Single().Email = targetUser.Email;

                    return Task.FromResult(1);
                }
            });

            mock.Setup(urm => urm.Delete(It.IsAny<string>())).Returns((string userEmail) =>
            {
                if (userEmail == null || !userList.Exists(c => c.Email == userEmail)) { return Task.FromResult(0); }
                else
                {
                    userList.RemoveAll(x => x.Email == userEmail);
                    return Task.FromResult(1);
                }
            });


            return mock;
        }

        public static Mock<IAddressRepository> GetAddressRepository(List<Address> addressList)
        {
            var mock = new Mock<IAddressRepository>();

            mock.Setup(arm => arm.GetAll()).ReturnsAsync(() => addressList);

            mock.Setup(arm => arm.Get(It.IsAny<Guid>())).Returns((Guid Id) =>
            {
                Address? address = addressList.FirstOrDefault(a => a.Id == Id);
                return Task.FromResult(address);
            });

            mock.Setup(arm => arm.Insert(It.IsAny<Address>())).Returns((Address newAddress) =>
            {
                if (addressList.Exists(a => a.Id == newAddress.Id)) { return Task.FromResult(0); }

                else { addressList.Add(newAddress); return Task.FromResult(1); }
            });

            mock.Setup(arm => arm.Update(It.IsAny<Address>())).Returns((Address targetAddress) =>
            {
                if (!addressList.Exists(a => a.Id == targetAddress.Id)) { return Task.FromResult(0); }
                else
                {
                    var orginal = addressList.Where(a => a.Id == targetAddress.Id).Single().Email = targetAddress.Email;

                    return Task.FromResult(1);
                }
            });

            mock.Setup(arm => arm.Delete(It.IsAny<Guid>())).Returns((Guid addressId) =>
            {
                if (!addressList.Exists(a => a.Id == addressId)) { return Task.FromResult(0); }
                else
                {
                    addressList.RemoveAll(a => a.Id == addressId);
                    return Task.FromResult(1);
                }
            });

            return mock;
        }

        public static Mock<IProductRepository> GetProductRepository(List<Product> productList)
        {
            var mock = new Mock<IProductRepository>();

            mock.Setup(arm => arm.GetAll()).ReturnsAsync(() => productList);

            mock.Setup(crm => crm.Get(It.IsAny<string>())).Returns((string productId) =>
            {
                Product? product = productList.FirstOrDefault(p => p.Id == productId);
                return Task.FromResult(product);
            });

            mock.Setup(arm => arm.Insert(It.IsAny<Product>())).Returns((Product newProduct) =>
            {
                if (productList.Exists(p => p.Id == newProduct.Id))
                {
                    throw new Exception("Product already exists");
                }
                else
                {
                    productList.Add(newProduct);
                    return Task.FromResult(1);
                }
            });

            mock.Setup(crm => crm.Update(It.IsAny<Product>())).Returns((Product targetProduct) =>
            {
                var originalProduct = productList.FirstOrDefault(p => p.Id == targetProduct.Id);

                if (originalProduct == null) { return Task.FromResult(0); }

                // use reflection to update all properties
                var properties = typeof(Product).GetProperties().Where(prop => prop.CanRead && prop.CanWrite);
                foreach (var property in properties)
                {
                    var targetValue = property.GetValue(targetProduct);
                    if (targetValue != null)
                    {
                        property.SetValue(originalProduct, targetValue);
                    }
                }

                return Task.FromResult(1);
            });

            mock.Setup(crm => crm.Delete(It.IsAny<string>())).Returns((string productId) =>
            {
                var productToRemove = productList.FirstOrDefault(p => p.Id == productId);
                if (productToRemove == null || !productList.Exists(p => p.Id == productId)) { return Task.FromResult(0); }
                else
                {
                    productList.Remove(productToRemove);
                    return Task.FromResult(1);
                }
            });

            return mock;
        }

        public static Mock<IOrderRepository> GetOrderRepository(List<Order> orderList)
        {
            var mock = new Mock<IOrderRepository>();

            mock.Setup(orm => orm.GetAll()).ReturnsAsync(() => orderList);

            mock.Setup(orm => orm.Get(It.IsAny<Guid>())).Returns((Guid Id) =>
            {
                Order? order = orderList.FirstOrDefault(o => o.Id == Id);
                return Task.FromResult(order);
            });

            mock.Setup(orm => orm.Insert(It.IsAny<Order>())).Returns((Order newOrder) =>
            {
                if (orderList.Exists(o => o.Id == newOrder.Id)) { return Task.FromResult(0); }

                else { orderList.Add(newOrder); return Task.FromResult(1); }
            });

            mock.Setup(orm => orm.Update(It.IsAny<Order>())).Returns((Order targetOrder) =>
            {
                if (!orderList.Exists(o => o.Id == targetOrder.Id)) { return Task.FromResult(0); }
                else
                {
                    var orginal = orderList.Where(o => o.Id == targetOrder.Id).Single().OrderStatus = targetOrder.OrderStatus;

                    return Task.FromResult(1);
                }
            });

            mock.Setup(orm => orm.Delete(It.IsAny<Guid>())).Returns((Guid orderId) =>
            {
                if (!orderList.Exists(o => o.Id == orderId)) { return Task.FromResult(0); }
                else
                {
                    orderList.RemoveAll(o => o.Id == orderId);
                    return Task.FromResult(1);
                }
            });

            return mock;
        }

    }
}
