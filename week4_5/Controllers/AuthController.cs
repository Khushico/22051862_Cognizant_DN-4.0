using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace fifth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // This controller can be accessed without authentication
    public class AuthController : ControllerBase
    {
        [HttpGet]
        [Route("GenerateToken")]
        public IActionResult GenerateToken(int userId = 1, string userRole = "Admin")
        {
            try
            {
                string token = GenerateJSONWebToken(userId, userRole);
                return Ok(new { 
                    Token = token, 
                    Message = "Token generated successfully!",
                    UserId = userId,
                    UserRole = userRole,
                    ExpiresIn = "10 minutes"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GenerateTokenPOC")]
        public IActionResult GenerateTokenPOC(int userId = 2)
        {
            try
            {
                string token = GenerateJSONWebToken(userId, "POC");
                return Ok(new { 
                    Token = token, 
                    Message = "POC Token generated successfully!",
                    UserId = userId,
                    UserRole = "POC",
                    ExpiresIn = "10 minutes"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GenerateExpiredToken")]
        public IActionResult GenerateExpiredToken(int userId = 3, string userRole = "Admin")
        {
            try
            {
                string token = GenerateJSONWebTokenWithExpiry(userId, userRole, 2); // 2 minutes
                return Ok(new { 
                    Token = token, 
                    Message = "Token with 2 minutes expiry generated!",
                    UserId = userId,
                    UserRole = userRole,
                    ExpiresIn = "2 minutes"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        private string GenerateJSONWebToken(int userId, string userRole)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysuperdupersecret_make_it_long_for_security_jwt_token"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, userRole),
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Name, $"User{userId}"),
                new Claim("TokenType", "Standard")
            };

            var token = new JwtSecurityToken(
                issuer: "mySystem",
                audience: "myUsers",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10), // 10 minutes valid
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJSONWebTokenWithExpiry(int userId, string userRole, int expiryMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysuperdupersecret_make_it_long_for_security_jwt_token"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, userRole),
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Name, $"User{userId}"),
                new Claim("TokenType", "ShortExpiry")
            };

            var token = new JwtSecurityToken(
                issuer: "mySystem",
                audience: "myUsers",
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}