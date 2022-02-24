using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RanobeNet.Data;
using RanobeNet.Utils;

namespace RanobeNet.Controllers
{
    [Route("")]
    [ApiController]
    [Produces("application/json")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult HelthCheck()
        {
            return Ok(new { status = "ok" });
        }
    }
}
