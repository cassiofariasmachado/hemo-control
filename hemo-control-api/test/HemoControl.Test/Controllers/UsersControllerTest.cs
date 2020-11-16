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
using HemoControl.Test.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HemoControl.Services;
using HemoControl.Models.Infusions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HemoControl.Test.Extensions;

namespace HemoControl.Test.Controllers
{
    public class UsersControllerTest
    {
        private readonly IPasswordService _passwordService;
        private readonly IAccessTokenService _accessTokenService;
        private readonly AccessTokenSettings _accessTokenSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersControllerTest()
        {
            _passwordService = A.Fake<IPasswordService>();
            _accessTokenService = A.Fake<IAccessTokenService>();
            _accessTokenSettings = new AccessTokenSettings { ExpiresIn = 1000 };
            _httpContextAccessor = A.Fake<IHttpContextAccessor>();
            _httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            _httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();
        }

        [Fact]
        public async Task RegisterUserShouldSaveCorrectly()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(RegisterUserShouldSaveCorrectly));

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
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings, _httpContextAccessor);

                response = await controller.RegisterAsync(request, default(CancellationToken));
            }

            response.AssertIsCreatedObjectResult<RegisterUserResponse>(async registerUserResponse =>
            {
                using (var context = new HemoControlContext(options))
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                    user.AssertRequest(request);
                }
            });
        }

        [Fact]
        public async Task RegisterUserReturnsBadRequestWhenUsernameAlreadyInUse()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(RegisterUserReturnsBadRequestWhenUsernameAlreadyInUse));

            var request = new RegisterUserRequest
            {
                Name = "Cássio",
                LastName = "Farias Machado",
                Email = "cfariasm@gmail.com",
                Username = "cassiofariasmachado",
                Password = "12345678"
            };

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(UserData.User);
                await context.SaveChangesAsync();
            }

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings, _httpContextAccessor);

                var response = await controller.RegisterAsync(request, default(CancellationToken));

                response.AssertIsBadRequestObjectResult<ErrorResponse>(errorResponse =>
                {
                    Assert.Equal("Username already in use", errorResponse.Message);
                });
            }
        }

        [Fact]
        public async Task GetUserShouldReturnsUserCorrectly()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(GetUserShouldReturnsUserCorrectly));

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(UserData.User);
                await context.SaveChangesAsync();
            }

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings, _httpContextAccessor);

                response = await controller.GetAsync(1, default(CancellationToken));
            }

            response.AssertIsOkObjectResult<UserResponse>(userResponse =>
            {
                UserData.User.AssertResponse(userResponse);
            });
        }

        [Fact]
        public async Task GetUserShouldReturnsNotFoundWhenUserNotExists()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(GetUserShouldReturnsNotFoundWhenUserNotExists));

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings, _httpContextAccessor);

                response = await controller.GetAsync(1, default(CancellationToken));
            }

            response.AssertIsNotFoundObjectResult<ErrorResponse>(errorResponse =>
            {
                Assert.Equal("User not found", errorResponse.Message);
            });
        }

        [Fact]
        public async Task LoginShouldReturnsUsernameOrPasssworInvalidsWhenItNotMatch()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(LoginShouldReturnsUsernameOrPasssworInvalidsWhenItNotMatch));

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(UserData.User);
                await context.SaveChangesAsync();
            }

            var request = new LoginRequest
            {
                Username = "cassiofariasmachado",
                Password = "12345678"
            };

            A.CallTo(() => _passwordService.Verify(A<string>.Ignored, A<string>.Ignored))
                .Returns(false);

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings, _httpContextAccessor);

                var response = await controller.LoginAsync(request);

                response.AssertIsBadRequestObjectResult<ErrorResponse>(errorResponse =>
                {
                    Assert.Equal("Invalid username or password", errorResponse.Message);
                });
            }
        }

        [Fact]
        public async Task LoginShouldReturnsNotFoundWhenUserNotRegistered()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(LoginShouldReturnsNotFoundWhenUserNotRegistered));

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(UserData.User);
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

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings, _httpContextAccessor);

                var response = await controller.LoginAsync(request);

                response.AssertIsOkObjectResult<AccessTokenResponse>(accessTokenResponse =>
                {
                    Assert.Equal("TOKEN", accessTokenResponse.AccessToken);
                    Assert.Equal("Bearer", accessTokenResponse.Type);
                    Assert.Equal(_accessTokenSettings.ExpiresIn, accessTokenResponse.ExpiresIn);
                });
            }
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

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings, _httpContextAccessor);

                var response = await controller.LoginAsync(request);

                response.AssertIsNotFoundObjectResult<ErrorResponse>(errorResponse =>
                {
                    Assert.Equal("User not registered", errorResponse.Message);
                });
            }
        }

        [Fact]
        public async Task GetInfusionsShouldReturnInfusions()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(GetInfusionsShouldReturnInfusions));

            var identity = AccessTokenService.CreateIdentity("cassiofariasmachado");

            A.CallTo(() => _httpContextAccessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(identity));

            using (var context = new HemoControlContext(options))
            {
                await context.AddAsync(UserData.User);
                await context.AddRangeAsync(InfusionData.Infusions);
                await context.SaveChangesAsync();
            }

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings, _httpContextAccessor);

                response = await controller.GetInfusionsAsync(default(CancellationToken));
            }

            response.AssertIsOkObjectResult<IEnumerable<InfusionResponse>>(async infusionsResponse =>
            {
                using (var context = new HemoControlContext(options))
                {
                    var infusions = await context.Infusions
                        .Where(i => i.User.Username == "cassiofariasmachado")
                        .ToListAsync();

                    infusions.AssertResponse(infusionsResponse);
                }
            });
        }

        [Fact]
        public async Task GetInfusionsShouldReturnEmptyWhenNotExistsInfusions()
        {
            var options = EntityFrameworkUtils.CreateInMemoryDbOptions<HemoControlContext>(nameof(GetInfusionsShouldReturnEmptyWhenNotExistsInfusions));

            var identity = AccessTokenService.CreateIdentity("cassiofariasmachado");

            A.CallTo(() => _httpContextAccessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(identity));

            IActionResult response;

            using (var context = new HemoControlContext(options))
            {
                var controller = new UsersController(context, _passwordService, _accessTokenService, _accessTokenSettings, _httpContextAccessor);

                response = await controller.GetInfusionsAsync(default(CancellationToken));
            }

            response.AssertIsOkObjectResult<IEnumerable<InfusionResponse>>(
                infusionsResponse => Assert.Empty(infusionsResponse)
            );
        }
    }
}