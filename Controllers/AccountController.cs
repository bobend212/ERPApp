using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ERPApp.Helpers;
using ERPApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ERPApp.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel formData)
        {
            List<string> errorList = new List<string>();

            var user = new IdentityUser
            {
                Email = formData.Email,
                UserName = formData.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, formData.Password);

            if(result.Succeeded)
            {
                //create new user and assign default role
                await _userManager.AddToRoleAsync(user, "User");
                //here I can add sending confirmation email method
                return Ok(new { username = user.UserName, email = user.Email, status = 1, message = "Registration successfull" });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorList.Add(error.Description);
                }
            }
            return BadRequest(new JsonResult(errorList));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel formData)
        {
            var user = await _userManager.FindByNameAsync(formData.Username);

            var roles = await _userManager.GetRolesAsync(user);

            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));

            double tokenExpireTime = Convert.ToDouble(_appSettings.ExpireTime);

            if(user != null && await _userManager.CheckPasswordAsync(user, formData.Password))
            {
                //generate token if credentials are valid
                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, formData.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim("LoggedOn", DateTime.Now.ToString())
                    }),
                    SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _appSettings.Site,
                    Audience = _appSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpireTime)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new { token = tokenHandler.WriteToken(token), expirationTime = token.ValidTo, username = user.UserName, userRole = roles.FirstOrDefault() });
            }

            ModelState.AddModelError("", "Invalid Username or Password");
            return Unauthorized(new { LoginError = "Please check your credentials - invalid username or password" });
        }




    }
}