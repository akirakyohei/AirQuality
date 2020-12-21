using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AirQualityService.Data.Interface;
using AirQualityService.Helpers;
using AirQualityService.Helpers.@interface;
using AirQualityService.Model;
using AirQualityService.ViewModels;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AirQualityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        private readonly IRsaHelper rsaHelper;

        public AuthController(IAccountRepository accountRepository, IRsaHelper rsaHelper)
        {
            this.accountRepository = accountRepository;
            this.rsaHelper = rsaHelper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request.");
            }

            try
            {

                string password = rsaHelper.Decrypt(user.Password);

                var u = accountRepository.GetAccount();

                if (user.Username == u.Username && BCrypt.Net.BCrypt.Verify(password, u.Password))
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
            catch (Exception)
            {
                // Log ex 
                return BadRequest();
            }

        }

        [HttpGet("logout"), Authorize(Roles = "Manager")]
        public IActionResult Logout()
        {
            return Unauthorized();
        }


        [HttpPut("updateAccount"), Authorize(Roles = "Manager")]
        public IActionResult UpdateAccount([FromBody] AccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string password = rsaHelper.Decrypt(accountVM.lastPassword);
                    var u = accountRepository.GetAccount();

                    if (BCrypt.Net.BCrypt.Verify(password, u.Password))
                    {

                        Console.WriteLine(accountVM.username);
                        string newPassword = rsaHelper.Decrypt(accountVM.password);
                        var account = new Account()
                        {
                            UserId = u.UserId,
                            Password = BCrypt.Net.BCrypt.HashPassword(newPassword),
                            Username = accountVM.username,
                            Modified = DateTime.Now
                        };
                        var result = accountRepository.UpdateAccount(account);

                        if (result)
                        {
                            return Ok();
                        }
                        else
                        {
                            return BadRequest();
                        }

                    }
                    return BadRequest();

                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }
            return BadRequest();

        }
    }
}