using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RanobeNet.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index(long id)
        {
            return Ok();
        }
    }
}
