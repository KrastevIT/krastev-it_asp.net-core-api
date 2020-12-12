using Krastev_It_API.Data;
using Krastev_It_API.Models.Sites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Krastev_It_API.Controllers
{
    public class SiteController : ApiContoller
    {
        private static readonly string RoleAdmin = "Admin";

        private readonly KrastevItDbContext db;
        private readonly UserManager<User> userManager;

        public SiteController(KrastevItDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        [Route(nameof(CreateQuestion))]
        public ActionResult<SiteQuestionModel> CreateQuestion(SiteQuestionModel model)
        {
            var question = new SiteQuestion
            {
                Category = model.Category,
                Name = model.Name,
                Email = model.Email,
                Description = model.Description
            };

            this.db.SiteQuestions.Add(question);
            this.db.SaveChanges();

            return Ok(model);

        }

        [HttpGet(nameof(GetQuestions) + "/{username}")]
        public async Task<ActionResult> GetQuestions(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await this.userManager.IsInRoleAsync(user, RoleAdmin);
            if (isAdmin)
            {
                var questions = this.db.SiteQuestions.ToList();
                return Ok(questions);
            }

            return Unauthorized();
        }
    }
}
