using HemoControl.Controllers;
using HemoControl.Database;
using HemoControl.Test.Utils;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HemoControl.Models.Users.Register;
using FakeItEasy;
using HemoControl.Interfaces.Services;
using HemoControl.Settings;
using System.Threading.Tasks;
using System.Threading;

namespace HemoControl.Test.Controllers
{
    public class UsersControllerTest
    {
        [Fact]
        public async Task RegisterUserShouldSaveTheUser()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>("Update");

            var request = new RegisterUserRequest
            {
                Name = "Cássio",
                LastName = "Farias Machado",
                Email = "cfariasm@gmail.com",
                Username = "cfariasm",
                Password = "12345678"
            };

            var fakePasswordService = A.Fake<IPasswordService>();
            var fakeAccessTokenService = A.Fake<IAccessTokenService>();
            var fakeAccessTokenSettings = new AccessTokenSettings();

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, fakePasswordService, fakeAccessTokenService, fakeAccessTokenSettings);

                response = await controller.RegisterAsync(request, default(CancellationToken));
            }

            using (var context = new HemoControlContext(options))
            {
                var user = context.Users.FirstOrDefault(u => u.Username == request.Username);

                Assert.Equal(request.Name, user.Name);
                Assert.Equal(request.LastName, user.LastName);
                Assert.Equal(request.Email, user.Email);
                Assert.Equal(request.Username, user.Username);
                Assert.NotEqual(request.Password, user.Password);

                Assert.IsType<CreatedResult>(response);
            }
        }
    }
}