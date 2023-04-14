using Moq;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Repositories;
using ShopBackend.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
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

        public static Mock<IAuthService> GetAuthService(List<Customer> customerList, Customer authenticatedCustomer, string testToken)
        {
            var mock = new Mock<IAuthService>();

            mock.Setup(asm => asm.AuthenticateUser(It.IsAny<LoginDto>())).Returns((LoginDto loginDto) =>
            {
                if (loginDto != null && customerList.Exists(c => c.Email == loginDto.Email && c.Password == loginDto.Password)) { return Task.FromResult(true); }
                else { return Task.FromResult(false); }
            });

            mock.Setup(asm => asm.GetEmailFromToken(It.IsAny<ClaimsPrincipal>())).Returns((ClaimsPrincipal claimsPrincipal) =>
            {
                return authenticatedCustomer.Email;
            });

            mock.Setup(asm => asm.CreateToken()).Returns(() =>
            {
                return testToken;
            });

            return mock;
        }

        public static Mock<IPasswordAuth> GetPasswordAuth()
        {
            var mock = new Mock<IPasswordAuth>();

            mock.Setup(asm => asm.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns((string password1, string password2) =>
            {
                return password1 == password2;
            });

            mock.Setup(asm => asm.GeneratePasswordHash(It.IsAny<string>())).Returns((string password) =>
            {
                return password;
            });

            mock.Setup(asm => asm.IsPasswordStrong(It.IsAny<string>())).Returns((string password) =>
            {
                Regex validPassword = new("^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-/*_]).{8,}$");
                return validPassword.IsMatch(password);
            });

            return mock;
        }

    }
}
