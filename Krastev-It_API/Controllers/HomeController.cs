using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krastev_It_API.Controllers
{
    public class HomeController : ApiContoller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("Works");
        }
    }
}
