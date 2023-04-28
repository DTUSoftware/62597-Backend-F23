using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using ShopBackend.Controllers;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Utils;

namespace ShopBackend.Test.ControllersTest
{
    /**
     * @author: Golbas Haidari
     * @date: 01-04-2023
     */
    public class TestCustomerController
    {
        /*
        private readonly List<Customer> customerList;
        private readonly CustomersController customersController;

        public TestCustomerController() {
            //Mutual Arrange
            customerList = DataHelper.GetFakeCustomerList();            
            var mock = MockIRepositories.GetCustomerRepository(customerList);
            customersController = new CustomersController(mock.Object);
        }

        [Fact]
        public async Task GetAllCustomers_onOk()
        {
            //Act
            var actionResult=  await customersController.Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            IEnumerable<CustomerDto> list = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task GetAllCustomers_onNotFound()
        {
            //Arrange
            var emptyCustomerList = new List<Customer>();
            var mock = MockIRepositories.GetCustomerRepository(emptyCustomerList);
            var Controller = new CustomersController(mock.Object);


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
            var actionResult= await customersController.Get(customerEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var customer = Assert.IsType<CustomerDto>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(customerList[0].Email, customer.Email);
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
            var newCustomer = new CreateCustomerDto { Email = "dg@gmail.com", Password = "1234" };

            //Act
            var actionResult = await customersController.Create(newCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<CreatedResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((CreatedResult)actionResult.Result).Value);
            Assert.Equal("Customer is inserted successfully!", msg);
        }

        [Fact]
        public async Task CreateCustomer_onBadRequest_EmptyEmail() 
        {
            //Arrange
            var newCustomer = new CreateCustomerDto { Email ="", Password = "5678" };

            //Act
            var actionResult = await customersController.Create(newCustomer);

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
            var newCustomer = new CreateCustomerDto { Email = "goli@gmail.com", Password = "5678" };

            //Act
            var actionResult = await customersController.Create(newCustomer);

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
            var targetCustomer = new CustomerDto { Email = "goli@gmail.com" };

            //Act
            var actionResult = await customersController.Update(targetCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer has been updated!", msg);
        }

        [Fact]
        public async Task UpdateCustomer_onBadRequest_EmptyEmail()
        {
            //Arrange
            var targetCustomer = new CustomerDto { Email = "" };

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
            var targetCustomer = new CustomerDto { Email = "David@gmail.com" };

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
            string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
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
         */
    }

}
