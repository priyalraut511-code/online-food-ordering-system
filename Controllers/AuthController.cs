using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodOrderSystem.Controllers.API
{
    [Route("api/food")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login()
        {
            // JWT Token Handler

            var tokenHandler =
                new JwtSecurityTokenHandler();

            // Secret Key

            var key =
                Encoding.ASCII.GetBytes
                ("THIS_IS_MY_SUPER_SECRET_KEY_FOR_JWT_12345vhjiihihihoijoiujoioijoiijiojio");

            // Token Descriptor

            var tokenDescriptor =
                new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity
                    (
                        new Claim[]
                        {
                            new Claim
                            (
                                ClaimTypes.Name,
                                "Admin"
                            )
                        }
                    ),

                    Expires =
                        DateTime.UtcNow.AddHours(1),

                    SigningCredentials =
                        new SigningCredentials
                        (
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature
                        )
                };

            // Create Token

            var token =
                tokenHandler.CreateToken
                (tokenDescriptor);

            // Return Token

            return Ok(new
            {
                token =
                    tokenHandler.WriteToken(token)
            });
        }
    }
}