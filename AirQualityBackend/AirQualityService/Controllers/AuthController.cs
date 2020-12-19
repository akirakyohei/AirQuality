using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AirQualityService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AirQualityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request.");
            }
            if (user.Username == "admin" && user.Password == "admin")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("indfivfglfivfusdfdg"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOption = new JwtSecurityToken(
                    issuer: "jndjfhvdkf",
                    audience: "hfbvfhgvklnckjnrubsdx",
                    claims: new List<Claim>() {
                        new Claim(ClaimTypes.Name,"admin"),
                        new Claim(ClaimTypes.Role,"Manager")
                    },
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signinCredentials); ;
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOption);
                return Ok(new { Token = token });

            }
            else
            {
                return Unauthorized();
            }
        }
    }
}