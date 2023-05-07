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
    public class TestUserController
    {

        private readonly List<User> userList;
        private readonly UsersController usersController;
        IPasswordAuth passwordAuth;

        public TestUserController()
        {
            //Mutual Arrange
            userList = DataHelper.GetFakeUserList();
            passwordAuth = new PasswordAuth();
            var urMock = MockIRepositories.GetUserRepository(userList);
            var arMock = MockIRepositories.GetAuthService(userList);

            usersController = new UsersController(urMock.Object, arMock.Object, passwordAuth);
        }


        [Fact]
        public async Task GetAllUsers_OnOk()
        {
            //Act
            var actionResult = await usersController.Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            List<UserDto> list = Assert.IsAssignableFrom<List<UserDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(2, list.Count);
            Assert.Equal(2, list[0].Links.Count);
        }

        [Fact]
        public async Task GetAllUsers_onNotFound()
        {
            //Arrange
            var emptyCustomerList = new List<User>();
            var mock = MockIRepositories.GetUserRepository(emptyCustomerList);
            var arMock = MockIRepositories.GetAuthService(emptyCustomerList);
            var Controller = new UsersController(mock.Object, arMock.Object, passwordAuth);


            //Act
            var actionResult = await Controller.Get();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified users does not exist!", msg);
        }

        [Theory]
        [InlineData("goli@gmail.com")]
        public async Task GetUserByEmail_onOk(string userEmail)
        {
            //Act
            var actionResult = await usersController.Get(userEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var user = Assert.IsType<UserDto>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(userList[0].Email, user.Email);
            Assert.Equal(2, user.Links.Count);
        }

        [Theory]
        [InlineData("Jeppe@gmail.com")]
        public async Task GetUserByEmail_onNotFound(string userEmail)
        {
            //Act
            var actionResult = await usersController.Get(userEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("The specified user does not exist!", msg);
        }

        [Fact]
        public async Task CreateUser_onOk()
        {
            //Arrange
            var newUser = new CreateUserDto { Email = "dg@gmail.com", Password = "Dtu12345678#" };

            //Act
            var actionResult = await usersController.Register(newUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var res = ((OkObjectResult)actionResult.Result).Value;
            string msg = Assert.IsType<string>(res!.GetType()!.GetProperty("Msg")!.GetValue(res!));
            Assert.Equal("User is inserted successfully!", msg);
        }

        [Fact]
        public async Task CreateUser_onBadRequest_WeakPassword()
        {
            //Arrange
            var newUser = new CreateUserDto { Email = "dg@gmail.com", Password = "1234" };

            //Act
            var actionResult = await usersController.Register(newUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var res = ((BadRequestObjectResult)actionResult.Result).Value;
            string msg = Assert.IsType<string>(res);
            Assert.Equal("The password must have at least 8 letters and contain at least one upper case letter, one lower case letter, one number, and one special character!", msg);
        }

        [Fact]
        public async Task CreateUser_onBadRequest_InvalidEmail()
        {
            //Arrange
            var newUser = new CreateUserDto { Email = "dggmail.com", Password = "Dtu12345678#" };

            //Act
            var actionResult = await usersController.Register(newUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Email format is not valid!", msg);
        }

        [Fact]
        public async Task CreateUser_onBadRequest_EmptyEmail()
        {
            //Arrange
            var newUser = new CreateUserDto { Email = "", Password = "Dtu12345678#" };

            //Act
            var actionResult = await usersController.Register(newUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("User email is required to register the user!", msg);
        }

        [Fact]
        public async Task CreateUser_onBadRequest_EmailExist()
        {
            //Arrange 
            var newUser = new CreateUserDto { Email = "goli@gmail.com", Password = "Dtu12345678#"};

            //Act
            var actionResult = await usersController.Register(newUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("This email is already in use!", msg);
        }


        [Fact]
        public async Task UpdateUser_onOk()
        {
            //Arrange 
            var targetUser = new UpdateUserDto { Email = "goli@gmail.com", Password = "Dtu12345678#" };

            //Act
            var actionResult = await usersController.Update(targetUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var links = Assert.IsType<List<Link>>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal(2, links.Count);
        }

        [Fact]
        public async Task UpdateUser_onBadRequest_WeakPassword()
        {
            //Arrange
            var targetUser = new UpdateUserDto { Email = "dg@gmail.com", Password = "1234" };

            //Act
            var actionResult = await usersController.Update(targetUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("The password must have at least 8 letters and contain at least one upper case letter, one lower case letter, one number, and one special character!", msg);
        }

        [Fact]
        public async Task UpdateUser_onBadRequest_InvalidEmail()
        {
            //Arrange
            var targetUser = new UpdateUserDto { Email = "dggmail.com", Password = "Dtu12345678#" };

            //Act
            var actionResult = await usersController.Update(targetUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("Email format is not valid!", msg);
        }

        [Fact]
        public async Task UpdateUser_onBadRequest_EmptyEmail()
        {
            //Arrange
            var targetUser = new UpdateUserDto { Email = "", Password = "Dtu12345678#" };

            //Act
            var actionResult = await usersController.Update(targetUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("User email is required to update the user!", msg);
        }

        [Fact]
        public async Task UpdateUser_onNotFound()
        {
            //Arrange
            var targetUser = new UpdateUserDto { Email = "David@gmail.com", Password = "Dtu12345678#" };

            //Act
            var actionResult = await usersController.Update(targetUser);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("User does not exsist!", msg);
        }

        [Theory]
        [InlineData("goli@gmail.com")]
        public async Task DeleteUser_onOk(string userEmail)
        {
            //Act
            var actionResult = await usersController.Delete(userEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var msg = Assert.IsType<string>(((OkObjectResult)actionResult.Result).Value);
            Assert.Equal("User has been deleted!", msg);
            Assert.Single(userList);
        }

        [Theory]
        [InlineData("")]
        public async Task DeleteUser_onBadRequest_EmptyEmail(string userEmail)
        {
            //Act
            var actionResult = await usersController.Delete(userEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((BadRequestObjectResult)actionResult.Result).Value);
            Assert.Equal("User email is required to delete the user!", msg);
            Assert.Equal(2, userList.Count);
        }

        [Theory]
        [InlineData("david@gmail.com")]
        public async Task DeleteUser_onNotFound(string userEmail)
        {
            //Act
            var actionResult = await usersController.Delete(userEmail);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            string msg = Assert.IsType<string>(((NotFoundObjectResult)actionResult.Result).Value);
            Assert.Equal("User does not exsist!", msg);
            Assert.Equal(2, userList.Count);
        }

    }

}
