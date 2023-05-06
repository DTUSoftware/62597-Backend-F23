using Microsoft.AspNetCore.Mvc;
using ShopBackend.Controllers;
using ShopBackend.Discoverabillity;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Security;

namespace ShopBackend.Test.ControllersTest
{
    /**
     * @author: Golbas Haidari
     * @date: 01-04-2023
     */
    public class TestCustomerController
    {

        private readonly List<Customer> customerList;
        private readonly CustomersController customersController;
        IPasswordAuth passwordAuth;

        public TestCustomerController()
        {
            //Mutual Arrange
            customerList = DataHelper.GetFakeCustomerList();
            passwordAuth = new PasswordAuth();
            var crmock = MockIRepositories.GetCustomerRepository(customerList);
            var arMock = MockIRepositories.GetAuthService(customerList);

            customersController = new CustomersController(crmock.Object, arMock.Object, passwordAuth);
        }


        [Fact]
        public async Task GetAllCustomers_onOk()
        {
            //Act
            var actionResult = await customersController.Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            List<CustomerDto> list = Assert.IsAssignableFrom<List<CustomerDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(2, list.Count);
            Assert.Equal(2, list[0].Links.Count);
        }

        [Fact]
        public async Task GetAllCustomers_onNotFound()
        {
            //Arrange
            var emptyCustomerList = new List<Customer>();
            var mock = MockIRepositories.GetCustomerRepository(emptyCustomerList);
            var arMock = MockIRepositories.GetAuthService(emptyCustomerList);
            var Controller = new CustomersController(mock.Object, arMock.Object, passwordAuth);


            //Act
            var actionResult = await Controller.Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified customers does not exist!", msg);
        }

        [Theory]
        [InlineData("goli@gmail.com")]
        public async Task GetCustomerByEmail_onOk(string customerEmail)
        {
            //Act
            var actionResult = await customersController.Get(customerEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var customer = Assert.IsType<CustomerDto>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(customerList[0].Email, customer.Email);
            Assert.Equal(2, customer.Links.Count);
        }

        [Theory]
        [InlineData("Jeppe@gmail.com")]
        public async Task GetCustomerByEmail_onNotFound(string customerEmail)
        {
            //Act
            var actionResult = await customersController.Get(customerEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified customer does not exist!", msg);
        }

        [Fact]
        public async Task CreateCustomer_onOk()
        {
            //Arrange
            var newCustomer = new CreateCustomerDto { Email = "dg@gmail.com", Password = "Dtu1234#" };

            //Act
            var actionResult = await customersController.Register(newCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var res = ((OkObjectResult)actionResult.Result).Value;
            string msg = Assert.IsType<string>(res.GetType().GetProperty("Msg").GetValue(res));
            Assert.Equal("Customer is inserted successfully!", msg);
        }

        [Fact]
        public async Task CreateCustomer_onBadRequest_WeakPassword()
        {
            //Arrange
            var newCustomer = new CreateCustomerDto { Email = "dg@gmail.com", Password = "1234" };

            //Act
            var actionResult = await customersController.Register(newCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var res = ((BadRequestObjectResult)actionResult.Result).Value;
            string msg = Assert.IsType<string>(res);
            Assert.Equal("The password must have at least 8 letters and contain at least one upper case letter, one lower case letter, one number, and one special character!", msg);
        }

        [Fact]
        public async Task CreateCustomer_onBadRequest_InvalidEmail()
        {
            //Arrange
            var newCustomer = new CreateCustomerDto { Email = "dggmail.com", Password = "Dtu5678#" };

            //Act
            var actionResult = await customersController.Register(newCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Email format is not valid!", msg);
        }

        [Fact]
        public async Task CreateCustomer_onBadRequest_EmptyEmail()
        {
            //Arrange
            var newCustomer = new CreateCustomerDto { Email = "", Password = "Dtu5678#" };

            //Act
            var actionResult = await customersController.Register(newCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer email is required to register the customer!", msg);
        }

        [Fact]
        public async Task CreateCustomer_onBadRequest_EmailExist()
        {
            //Arrange 
            var newCustomer = new CreateCustomerDto { Email = "goli@gmail.com", Password = "Dtu5678#" };

            //Act
            var actionResult = await customersController.Register(newCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("This email is already in use!", msg);
        }


        [Fact]
        public async Task UpdateCustomer_onOk()
        {
            //Arrange 
            var targetCustomer = new UpdateCustomerDto { Email = "goli@gmail.com", Password = "Dtu1234#" };

            //Act
            var actionResult = await customersController.Update(targetCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var links = Assert.IsType<List<Link>>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(2, links.Count);
        }

        [Fact]
        public async Task UpdateCustomer_onBadRequest_WeakPassword()
        {
            //Arrange
            var targetCustomer = new UpdateCustomerDto { Email = "dg@gmail.com", Password = "1234" };

            //Act
            var actionResult = await customersController.Update(targetCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("The password must have at least 8 letters and contain at least one upper case letter, one lower case letter, one number, and one special character!", msg);
        }

        [Fact]
        public async Task UpdateCustomer_onBadRequest_InvalidEmail()
        {
            //Arrange
            var targetCustomer = new UpdateCustomerDto { Email = "dggmail.com", Password = "Dtu1234#" };

            //Act
            var actionResult = await customersController.Update(targetCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Email format is not valid!", msg);
        }

        [Fact]
        public async Task UpdateCustomer_onBadRequest_EmptyEmail()
        {
            //Arrange
            var targetCustomer = new UpdateCustomerDto { Email = "", Password = "Dtu1234#" };

            //Act
            var actionResult = await customersController.Update(targetCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer email is required to update the customer!", msg);
        }

        [Fact]
        public async Task UpdateCustomer_onNotFound()
        {
            //Arrange
            var targetCustomer = new UpdateCustomerDto { Email = "David@gmail.com", Password = "Dtu1234#" };

            //Act
            var actionResult = await customersController.Update(targetCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer does not exsist!", msg);
        }

        [Theory]
        [InlineData("goli@gmail.com")]
        public async Task DeleteCustomer_onOk(string customerEmail)
        {
            //Act
            var actionResult = await customersController.Delete(customerEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer has been deleted!", msg);
            Assert.Single(customerList);
        }

        [Theory]
        [InlineData("")]
        public async Task DeleteCustomer_onBadRequest_EmptyEmail(string customerEmail)
        {
            //Act
            var actionResult = await customersController.Delete(customerEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer email is required to delete the customer!", msg);
            Assert.Equal(2, customerList.Count);
        }

        [Theory]
        [InlineData("david@gmail.com")]
        public async Task DeleteCustomer_onNotFound(string customerEmail)
        {
            //Act
            var actionResult = await customersController.Delete(customerEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer does not exsist!", msg);
            Assert.Equal(2, customerList.Count);
        }

    }

}
