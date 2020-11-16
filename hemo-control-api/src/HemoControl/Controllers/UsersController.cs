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
using HemoControl.Models.Infusions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(
            HemoControlContext context,
            IPasswordService passwordService,
            IAccessTokenService accessTokenService,
            AccessTokenSettings accessTokenSettings,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _context = context;
            _passwordService = passwordService;
            _accessTokenService = accessTokenService;
            _accessTokenSettings = accessTokenSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get an access token for the application
        /// </summary>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == default)
                return NotFound(new ErrorResponse { Message = "User not found" });

            return Ok(UserResponse.Map(user));
        }

        /// <summary>
        /// List user infusions
        /// </summary>
        [HttpGet("infusions")]
        public async Task<IActionResult> GetInfusionsAsync(CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var infusions = await _context.Infusions
                .Where(i => i.User.Username == username)
                .ToListAsync(cancellationToken);

            return Ok(infusions.Select(InfusionResponse.Map));
        }
    }
}