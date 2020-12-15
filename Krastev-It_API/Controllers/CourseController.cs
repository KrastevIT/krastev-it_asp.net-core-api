using Krastev_It_API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krastev_It_API.Controllers
{
    public class CourseController : ApiContoller
    {
        private readonly KrastevItDbContext db;
        private readonly UserManager<User> userManager;

        public CourseController(KrastevItDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        [Route(nameof(GetCourses))]
        public ActionResult GetCourses()
        {
            return Ok(this.db.Courses.ToList());
        }

        [HttpGet(nameof(GetLecturesByCourseId) + "/{id}")]
        public ActionResult GetLecturesByCourseId(int id)
        {
            var lectures = this.db.Lectures.Where(x => x.CourseId == id).ToList();
            return Ok(lectures);
        }

        [HttpGet(nameof(GetLectureByLectureId) + "/{id}")]
        public ActionResult GetLectureByLectureId(int id)
        {
            var lecture = this.db.Lectures.FirstOrDefault(x => x.Id == id);
            return Ok(lecture);
        }

    }
}
