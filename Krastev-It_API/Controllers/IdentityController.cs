using Krastev_It_API.Data;
using Krastev_It_API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Krastev_It_API.Controllers
{
    public class IdentityController : ApiContoller
    {
        private static readonly string Role = "Admin";

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppSettings appSettings;

        public IdentityController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AppSettings> appSettings)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appSettings = appSettings.Value;
        }

        [Route(nameof(Register))]
        public async Task<ActionResult<LoginModel>> Register(RegisterModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (!await roleManager.RoleExistsAsync(Role))
            {
                await roleManager.CreateAsync(new IdentityRole(Role));
                await userManager.AddToRoleAsync(user, Role);
            }

            if (result.Succeeded)
            {
                return Ok(new LoginModel { UserName = model.UserName, Password = model.Password });
            }

            return BadRequest(result.Errors);
        }

        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized();
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            var modelModel = new LoginResponseModel
            {
                Username = user.UserName,
                Token = encryptedToken
            };

            return modelModel;
        }

    }
}
