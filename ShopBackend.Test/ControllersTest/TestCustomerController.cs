using Microsoft.AspNetCore.Mvc;
using ShopBackend.Controllers;
using ShopBackend.Dtos;
using ShopBackend.Models;

namespace ShopBackend.Test.ControllersTest
{
    /**
     * @author: Golbas Haidari
     * @date: 01-04-203
     */
    public class TestCustomerController
    {
        
        private readonly List<Customer> customerList;

        public TestCustomerController() {
            customerList= new List<Customer>
            {
                new Customer{Email="goli@gmail.com", Password="1234"},
                new Customer{Email="Karen@gmail.com", Password="1234"}
            };
        }

        private CustomersController Controller()
        {
            //Mutual Arrange
            var mockCustomerRepository = MockIRepositories.GetCustomerRepository(customerList);
            var mockAuthService = MockIRepositories.GetAuthService(customerList, customerList.First(), "TestToken");
            var mockPasswordAuth = MockIRepositories.GetPasswordAuth();
            var customerController = new CustomersController(mockCustomerRepository.Object, mockAuthService.Object, mockPasswordAuth.Object);
            return customerController;
        }

        [Fact]
        public async Task GetAllCustomers_onOk()
        {
            //Act
            var actionResult=  await Controller().Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            IEnumerable<CustomerDto> list = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(((OkObjectResult)actionResult.Result).Value );
            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task GetAllCustomers_onNotFound()
        {
            //Arrange
            var emptyCustomerList = new List<Customer>();
            var mockCustomerRepository = MockIRepositories.GetCustomerRepository(emptyCustomerList);
            var mockAuthService = MockIRepositories.GetAuthService(emptyCustomerList, emptyCustomerList.First(), "TestToken");
            var mockPasswordAuth = MockIRepositories.GetPasswordAuth();
            var Controller = new CustomersController(mockCustomerRepository.Object, mockAuthService.Object, mockPasswordAuth.Object);


            //Act
            var actionResult= await Controller.Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg= Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified customers does not exist!", msg);
        }

        [Fact]
        public async Task GetCustomerByEmail_onOk() 
        {   
            //Act
            var actionResult= await Controller().Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result );
            var customer=Assert.IsType<CustomerDto>( ((OkObjectResult)actionResult.Result).Value );
            Assert.Equal(customerList[0].Email, customer.Email);
        }

        [Fact]
        public async Task GetCustomerByEmail_onNotFound()
        {
            //Act
            var actionResult = await Controller().Get();

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
            var actionResult = await Controller().Register(newCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((OkResult)actionResult.Result).Value);
            Assert.Equal("Customer is inserted successfully!", msg);
        }

        /*
        [Fact]
        public async Task CreateCustomer_onBadRequest_NullEmail() 
        {
            //Arrange
            var newCustomer = new CreateCustomerDto { Email = null, Password = "5678" };

            //Act
            var actionResult = await Controller().Register(newCustomer);

            //Assert
            Assert.NotNull (actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer email is required to register the customer!", msg);
        }
        */

        [Fact]
        public async Task CreateCustomer_onBadRequest_EmailExist()
        {
            //Arrange 
            var newCustomer = new CreateCustomerDto { Email = "goli@gmail.com", Password = "5678" };

            //Act
            var actionResult = await Controller().Register(newCustomer);

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
            var targetCustomer = new UpdateCustomerDto{ Email = "goli@gmail.com" };

            //Act
            var actionResult = await Controller().Update(targetCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer has been updated!", msg);
        }

        /*
        [Fact]
        public async Task UpdateCustomer_onBadRequest_NullEmail()
        {
            //Arrange
            var targetCustomer = new UpdateCustomerDto { Email = null };

            //Act
            var actionResult = await Controller().Update(targetCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer email is required to update the customer!", msg);
        }
        */

        [Fact]
        public async Task UpdateCustomer_onNotFound()
        {
            //Arrange
            var targetCustomer = new UpdateCustomerDto { Email = "David@gmail.com" };

            //Act
            var actionResult = await Controller().Update(targetCustomer);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer does not exsist!", msg);
        }

        [Fact]
        public async Task DeleteCustomer_onOk()
        {
            //Act
            var actionResult = await Controller().Delete();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer has been deleted!", msg);
            Assert.Single(customerList);
        }

        /*
        [Theory]
        [InlineData(null)]
        public async Task DeleteCustomer_onBadRequest_NullEmail(string customerEmail)
        {
            //Act
            var actionResult = await Controller().Delete(customerEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Customer email is required to delete the customer!", msg);
            Assert.Equal(2, customerList.Count);
        }
        */

        /*
        [Theory]
        [InlineData("david@gmail.com")]
        public async Task DeleteCustomer_onNotFound(string customerEmail)
        {
            //Act
            var actionResult = await Controller().Delete(customerEmail);

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
