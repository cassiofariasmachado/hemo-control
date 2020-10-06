using HemoControl.Controllers;
using HemoControl.Database;
using HemoControl.Test.Utils;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HemoControl.Models.Users;
using FakeItEasy;
using HemoControl.Interfaces.Services;
using HemoControl.Settings;
using System.Threading.Tasks;
using System.Threading;
using HemoControl.Entities;
using HemoControl.Models.Errors;

namespace HemoControl.Test.Controllers
{
    public class UsersControllerTest
    {
        private readonly User _user = new User("Cássio", "Farias Machado", "cassiofariasmachado@yahoo.com", "cassiofariasmachado", "12345678");
        private readonly IPasswordService _passwordService;
        private readonly IAccessTokenService _accessTokenService;
        private readonly AccessTokenSettings _accessTokenSettings;

        public UsersControllerTest()
        {
            _passwordService = A.Fake<IPasswordService>();
            _accessTokenService = A.Fake<IAccessTokenService>();
            _accessTokenSettings = new AccessTokenSettings { ExpiresIn = 1000 };
        }

        [Fact]
        public async Task RegisterUserShouldSaveTheUser()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(RegisterUserShouldSaveTheUser));

            var request = new RegisterUserRequest
            {
                Name = "Cássio",
                LastName = "Farias Machado",
                Email = "cfariasm@gmail.com",
                Username = "cfariasm",
                Password = "12345678"
            };

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings);

                response = await controller.RegisterAsync(request, default(CancellationToken));
            }

            using (var context = new HemoControlContext(options))
            {
                Assert.IsType<CreatedResult>(response);

                var user = context.Users.FirstOrDefault(u => u.Username == request.Username);

                Assert.Equal(request.Name, user.Name);
                Assert.Equal(request.LastName, user.LastName);
                Assert.Equal(request.Email, user.Email);
                Assert.Equal(request.Username, user.Username);
                Assert.NotEqual(request.Password, user.Password);
            }
        }

        [Fact]
        public async Task GetUserShouldReturnsTheUser()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(GetUserShouldReturnsTheUser));

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(_user);
                await context.SaveChangesAsync();
            }

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings);

                response = await controller.GetAsync(1, default(CancellationToken));
            }

            var result = Assert.IsType<OkObjectResult>(response);

            var user = result.Value as UserResponse;

            Assert.Equal("Cássio", user.Name);
            Assert.Equal("Farias Machado", user.LastName);
            Assert.Equal("cassiofariasmachado@yahoo.com", user.Email);
            Assert.Equal("cassiofariasmachado", user.Username);
            Assert.Null(user.Birthday);
            Assert.Equal("cassiofariasmachado", user.Username);
            Assert.Null(user.Weigth);
        }

        [Fact]
        public async Task GetUserShouldReturnsNotFoundWhenUserNotExists()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(GetUserShouldReturnsNotFoundWhenUserNotExists));

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings);

                response = await controller.GetAsync(1, default(CancellationToken));
            }

            var result = Assert.IsType<NotFoundObjectResult>(response);
            var error = result.Value as ErrorResponse;

            Assert.Equal("User not found", error.Message);
        }

        [Fact]
        public async Task LoginShouldReturnsUsernameOrPasssworInvalidsWhenItNotMatch()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(LoginShouldReturnsUsernameOrPasssworInvalidsWhenItNotMatch));

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(_user);
                await context.SaveChangesAsync();
            }

            var request = new LoginRequest
            {
                Username = "cassiofariasmachado",
                Password = "12345678"
            };

            A.CallTo(() => _passwordService.Verify(A<string>.Ignored, A<string>.Ignored))
                .Returns(false);

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings);

                response = await controller.LoginAsync(request, default(CancellationToken));
            }

            var result = Assert.IsType<BadRequestObjectResult>(response);
            var error = result.Value as ErrorResponse;

            Assert.Equal("Invalid username or password", error.Message);
        }

        [Fact]
        public async Task LoginShouldReturnsNotFoundWhenUserNotRegistered()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(LoginShouldReturnsNotFoundWhenUserNotRegistered));

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(_user);
                await context.SaveChangesAsync();
            }

            A.CallTo(() => _passwordService.Verify(A<string>.Ignored, A<string>.Ignored))
                .Returns(true);

            A.CallTo(() => _accessTokenService.GenerateToken(A<User>.Ignored))
                .Returns("TOKEN");

            var request = new LoginRequest
            {
                Username = "cassiofariasmachado",
                Password = "12345678"
            };

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings);

                response = await controller.LoginAsync(request, default(CancellationToken));
            }

            var result = Assert.IsType<OkObjectResult>(response);
            var token = result.Value as AccessTokenResponse;

            Assert.Equal("TOKEN", token.AccessToken);
            Assert.Equal("Bearer", token.Type);
            Assert.Equal(_accessTokenSettings.ExpiresIn, token.ExpiresIn);
        }

        [Fact]
        public async Task LoginShouldReturnsAnAccessTokenWhenUsernameAndPasswordMatch()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(LoginShouldReturnsNotFoundWhenUserNotRegistered));

            var request = new LoginRequest
            {
                Username = "cassiofariasmachado",
                Password = "12345678"
            };

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings);

                response = await controller.LoginAsync(request, default(CancellationToken));
            }

            var result = Assert.IsType<NotFoundObjectResult>(response);
            var error = result.Value as ErrorResponse;

            Assert.Equal("User not registered", error.Message);
        }
    }
}