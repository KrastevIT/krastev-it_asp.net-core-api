using Krastev_It_API.Data;
using Krastev_It_API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Krastev_It_API.Controllers
{
    public class IdentityController : ApiContoller
    {
        private static readonly string RoleAdmin = "Admin";

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly KrastevItDbContext db;
        private readonly AppSettings appSettings;

        public IdentityController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AppSettings> appSettings,
            KrastevItDbContext db)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
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

            if (!await roleManager.RoleExistsAsync(RoleAdmin))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleAdmin));
                await userManager.AddToRoleAsync(user, RoleAdmin);
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

            var isAdmin = await this.userManager.IsInRoleAsync(user, RoleAdmin);
            if (isAdmin)
            {
                modelModel.IsAdmin = true;
            }

            return modelModel;
        }

        [Route(nameof(UpdateUser))]
        public async Task<ActionResult> UpdateUser(UpdateUserModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized();
            }
            else if (model.Password == null || model.NewPassword == null)
            {
                return BadRequest();
            }

            var isChange = await this.userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            if (isChange.Succeeded)
            {
                return Ok();
            }

            return BadRequest(isChange);
        }

        [HttpGet(nameof(GetUsers) + "/{username}")]
        public async Task<ActionResult> GetUsers(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await this.userManager.IsInRoleAsync(user, RoleAdmin);
            if (isAdmin)
            {
                var users = this.db.Users.ToList();

                var usersModel = new List<UsersModel>();

                foreach (var userDb in users)
                {
                    var model = new UsersModel
                    {
                        Username = userDb.UserName,
                        Email = userDb.Email
                    };

                    usersModel.Add(model);
                }

                return Ok(usersModel);
            }

            return Unauthorized();
        }

    }
}
