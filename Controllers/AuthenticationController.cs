using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using my_app_backend.Domain.AggregateModel.UserAggregate;
using my_app_backend.Domain.SeedWork.Models;
using my_app_backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace my_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationSettings _settings;
        public AuthenticationController(IOptions<AuthenticationSettings> options)
        {
            _settings = options.Value;
        }

        private List<User> _user = new List<User>()
        {
            new User
            {
                Id = 1,
                Username = "Alice",
                Password = "123456",
                Role = Constants.Roles.Admin,
                Description = "Admin Account"
            },
            new User
            {
                Id = 2,
                Username = "Bob",
                Password = "123456",
                Role = Constants.Roles.Normal,
                Description = "Normal Account"
            }
        };
        [HttpPost("login")]
        public ActionResult<ApiResponse<LoginRes>> Login([FromBody] LoginReq req)
        {
            var user = _user.FirstOrDefault(t => t.Username == req.Username && t.Password == req.Password);
            if (user == null)
            {
                return Ok(ApiResponse<LoginRes>.Error("Username or Password is incorrect!"));
            }

            var token = GenerateToken(user);

            return Ok(ApiResponse<LoginRes>.Ok(new LoginRes
            {
                Id = user.Id,
                Role = user.Role,
                Description = user.Description,
                Token = token,
                Username = req.Username
            }));
        }

        private string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_settings.SecurityKey);
            var secret = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            // Generate claims.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Create token options & generate token.
            var tokenOptions = new JwtSecurityToken(
                issuer: _settings.ValidIssuer,
                audience: _settings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_settings.ExpiryInMinutes),
                signingCredentials: signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
    }
}
