using BusinessLayer.Repository;
using BusinessLayer.RepositoryImplementation;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using ECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Org.BouncyCastle.Crypto.Macs;

namespace Testiing.Controllers
{
    public class UserControllerTest
    {
        private Mock<IUser> user = new Mock<IUser>();

        [Fact]
        public async void GetAllUsersTest()
        {
            //Arrange

            var usersDtoTest = new List<UserResponseDTO>()
            {
                new UserResponseDTO
                {
                          ID = 1,
                          Name = "Rajdeep Tiwari",
                         RoleName = "Admin",
                },
            };

            user.Setup(p => p.AllUsers()).ReturnsAsync(usersDtoTest);
            UsersController u = new UsersController(user.Object);
            var result = u.GetAll();
            //Assert.True(usersDtoTest.Equals(result));
            Assert.Equal(usersDtoTest, result.Result);
        }

        [Fact]
        public async void Adduser_WithoutError_Test()
        {
            //Arrenge
            UsersController u = new UsersController(user.Object);
            var UserTest = new UserDTO()
            {
                ID = 1,
                Name = "Test",
                Email = "Test@gmail.com",
            };

            user.Setup(x => x.AddUserasync(UserTest, 2)).ReturnsAsync(UserTest);
            var Suppiler = await u.AddSuppiler(UserTest);

            user.Setup(x => x.AddUserasync(UserTest, 3)).ReturnsAsync(UserTest);

            var customer = await u.AddCustomer(UserTest);

            //assert

            Assert.Equal(UserTest, customer);
            Assert.Equal(UserTest, Suppiler);
        }
    }
}