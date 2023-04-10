using Moq;
using ShopBackend.Models;
using ShopBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBackend.Test.ControllersTest
{
    public class MockIRepositories
    {
        public static Mock<ICustomerRepository> GetCustomerRepository(List<Customer> customerList)
        {
            var mock = new Mock<ICustomerRepository>();

            mock.Setup(crm => crm.GetAll()).ReturnsAsync(() => customerList);

            mock.Setup(crm => crm.Get(It.IsAny<string>())).Returns((string email) =>
            {
                Customer? customer = customerList.FirstOrDefault(c => c.Email == email);
                return Task.FromResult(customer);
            });

            mock.Setup(crm => crm.Insert(It.IsAny<Customer>())).Returns((Customer newCustomer) =>
            {
                if (newCustomer.Email == null || customerList.Exists(c => c.Email == newCustomer.Email)) { return Task.FromResult(0); }

                else { customerList.Add(newCustomer); return Task.FromResult(1); }
            });

            mock.Setup(crm => crm.Update(It.IsAny<Customer>())).Returns((Customer targetCustomer) =>
            {
                if (targetCustomer.Email == null || !customerList.Exists(c => c.Email == targetCustomer.Email)) { return Task.FromResult(0); }
                else
                {
                    var orginal = customerList.Where(c => c.Email == targetCustomer.Email).Single().Email = targetCustomer.Email;

                    return Task.FromResult(1);
                }
            });

            mock.Setup(crm => crm.Delete(It.IsAny<string>())).Returns((string customerEmail) =>
            {
                if (customerEmail == null || !customerList.Exists(c => c.Email == customerEmail)) { return Task.FromResult(0); }
                else
                {
                    customerList.RemoveAll(x => x.Email == customerEmail);
                    return Task.FromResult(1);
                }
            });


            return mock;
        }
    }
}
