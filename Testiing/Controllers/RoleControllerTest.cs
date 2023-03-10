using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using ECommerce.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testiing.Controllers
{
    public class RoleControllerTest
    {
        private Mock<IRoles> RoleMock = new Mock<IRoles>();

        [Fact]
        public async void AddRole_WithOutError_Test()
        {
            // Arrenge
            RoleDTO Role = new RoleDTO()
            {
                ID = 1,
                RoleName = "Test",
            };

            RoleMock.Setup(x => x.AddRolesAsync(Role, 3)).ReturnsAsync(Role);

            //act

            RolesController RoleCtor = new RolesController(RoleMock.Object);
            var result = await RoleCtor.AddRoles(Role);
            //Assert

            Assert.Equal(Role, result);
        }
    }
}