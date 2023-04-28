using Microsoft.AspNetCore.Mvc;
using ShopBackend.Controllers;
using ShopBackend.Dtos;
using ShopBackend.Models;

namespace ShopBackend.Test.ControllersTest
{
    /**
     * @author: Golbas Haidari
     * @date: 01-04-2023
     */
    public class TestAddressController
    {
        /*
        private readonly List<Address> addressList;
        private readonly AddressController addressController;

        public TestAddressController()
        {
            //Mutual Arrange
            addressList = DataHelper.GetFakeAddressList(); 
            var mock = MockIRepositories.GetAddressRepository(addressList);
            addressController = new AddressController(mock.Object);            
        }
        
        [Fact]
        public async Task GetAllAddresses_onOk()
        {
            //Act
            var actionResult = await addressController.Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            List<Address> list = Assert.IsAssignableFrom<List<Address>>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetAllAddresses_onNotFound()
        {
            //Arrange
            var emptyAddressList = new List<Address>();
            var mock = MockIRepositories.GetAddressRepository(emptyAddressList);
            var Controller = new AddressController(mock.Object);

            //Act
            var actionResult = await Controller.Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified addresses does not exist!", msg);
        }

        [Theory]
        [InlineData("1a1b1c1d-d9cb-469f-a165-70867728950e")]
        public async Task GetAddressByGuid_onOk(string Id)
        {
            //Arrange
            var addressId = Guid.Parse(Id);

            //Act            
            var actionResult = await addressController.Get(addressId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var customer = Assert.IsType<AddressDto>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(addressList[0].Email, customer.Email);
        }

        [Theory]
        [InlineData("3a3b3c3d-d9cb-469f-a165-70867728950e")]
        public async Task GetAddressByGuid_onNotFound(string Id)
        {
            //Arrange
            var addressId = Guid.Parse(Id);

            //Act            
            var actionResult = await addressController.Get(addressId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified address does not exist!", msg);
        }

        [Fact]
        public async Task CreateAddress_onOk()
        {
            //Arrange
            var newAddress = new CreateAddressDto
            {
                FirstName = "David",
                LastName = "Rasmusen",
                Email = "david@gmail.com",
                MobileNr = "12345678",
                Company = "DTU",
                VatNr = "12345678",
                Country = "Denmark",
                ZipCode = "2200",
                City = "Valby",
                Address1 = "Gammel kongvej 120",
                Address2 = "st, tv"
            };

            //Act
            var actionResult = await addressController.Create(newAddress);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<CreatedResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((CreatedResult)actionResult.Result).Value);
            Assert.Equal("Address is registered successfully.", msg);
        }

        [Fact]
        public async Task CreateAddress_onNotFound()
        {
            //Arrange
            var newAddress = new CreateAddressDto
            {
                FirstName = "David",
                LastName = "Rasmusen",
                Email = "david@gmail.com",
                MobileNr = "12345678",
                Company = "DTU",
                VatNr = "12345678",
                Country = "Denmark",
                ZipCode = "2200",
                City = "Valby",
                Address1 = "Gammel kongvej 120",
                Address2 = "st, tv"
            };

            //Act
            await addressController.Create(newAddress); //we create the address one time
            var actionResult = await addressController.Create(newAddress); // so for the second time cannot be added.

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("Address could not be registered!", msg);
        }

        [Fact]
        public async Task UpdateAddress_onOk()
        {
            //Arrange 
            var addressId = Guid.Parse("1a1b1c1d-d9cb-469f-a165-70867728950e");
            var targetAddres = new Address { Id = addressId, Email = "goli@student.dtu.dk" };

            //Act
            var actionResult = await addressController.Update(targetAddres);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal("Address has been updated!", msg);
        }

        [Fact]
        public async Task UpdateCustomer_onNotFound()
        {
            //Arrange
            var addressId = Guid.Parse("4a4b4c4d-d9cb-469f-a165-70867728950e");
            var targetAddres = new Address { Id = addressId, Email = "goli@student.dtu.dk" };

            //Act
            var actionResult = await addressController.Update(targetAddres);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified address could not be found!", msg);
        }

        [Theory]
        [InlineData("1a1b1c1d-d9cb-469f-a165-70867728950e")]
        public async Task DeleteAddress_onOk(string Id)
        {
            //Arrange 
            var addressId = Guid.Parse(Id);

            //Act
            var actionResult = await addressController.Delete(addressId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified address has been deleted!", msg);
            Assert.Single(addressList);
        }

        [Theory]
        [InlineData("4a4b4c4d-d9cb-469f-a165-70867728950e")]
        public async Task DeleteAddress_onNotFound(string Id)
        {
            //Arrange
            var addressId = Guid.Parse(Id);

            //Act
            var actionResult = await addressController.Delete(addressId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified address could not be found!", msg);
            Assert.Equal(2, addressList.Count);
        }
         */
    }

}
