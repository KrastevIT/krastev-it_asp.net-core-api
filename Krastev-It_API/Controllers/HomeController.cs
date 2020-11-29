using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krastev_It_API.Controllers
{
    public class HomeController : ApiContoller
    {
        //[Authorize]
        public ActionResult Get()
        {
            return Ok("Works");
        }
    }
}
