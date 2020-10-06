using HemoControl.Database;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HemoControl.Entities;
using HemoControl.Models.Users;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HemoControl.Models.Errors;
using HemoControl.Settings;
using HemoControl.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HemoControl.Controllers
{
    [Controller]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly HemoControlContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IAccessTokenService _accessTokenService;
        private readonly AccessTokenSettings _accessTokenSettings;

        public UsersController(
            HemoControlContext context,
            IPasswordService passwordService,
            IAccessTokenService accessTokenService,
            AccessTokenSettings accessTokenSettings
        )
        {
            _context = context;
            _passwordService = passwordService;
            _accessTokenService = accessTokenService;
            _accessTokenSettings = accessTokenSettings;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var userExists = _context.Users.Any(u => u.Username == request.Username);
            if (userExists)
                return BadRequest(new ErrorResponse { Message = "Username already in use" });

            var user = new User(request.Name, request.LastName, request.Email, request.Username, request.Password);

            await _context.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Created("/api/users", new RegisterUserResponse { Id = user.Id });
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == default)
                return NotFound(new ErrorResponse { Message = "User not found" });

            return Ok(new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Birthday = user.Birthday,
                Email = user.Email,
                Username = user.Username,
                Weigth = user.Weigth
            });
        }

        /// <summary>
        /// Get an access token for the application
        /// </summary>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == default)
                return NotFound(new ErrorResponse { Message = "User not registered" });

            bool validCredentials = _passwordService.Verify(request.Password, user.Password);

            if (!validCredentials)
                return BadRequest(new ErrorResponse { Message = "Invalid username or password" });

            var accessToken = _accessTokenService.GenerateToken(user);

            return Ok(new AccessTokenResponse(accessToken, _accessTokenSettings));
        }
    }

}