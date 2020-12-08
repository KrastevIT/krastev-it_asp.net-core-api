using Krastev_It_API.Data;
using Krastev_It_API.Models.Sites;
using Microsoft.AspNetCore.Mvc;

namespace Krastev_It_API.Controllers
{
    public class SiteController : ApiContoller
    {
        private readonly KrastevItDbContext db;

        public SiteController(KrastevItDbContext db)
        {
            this.db = db;
        }

        [Route(nameof(CreateQuestion))]
        public ActionResult<SiteQuestionModel> CreateQuestion(SiteQuestionModel model)
        {
            var question = new SiteQuestion
            {
                Category = model.Category,
                Username = model.Username,
                Email = model.Email,
                Description = model.Description
            };

            this.db.SiteQuestions.Add(question);
            this.db.SaveChanges();

            return Ok(model);

        }
    }
}
