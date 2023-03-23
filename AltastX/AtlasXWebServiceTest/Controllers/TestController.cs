using Microsoft.AspNetCore.Mvc;

namespace atlastX.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        public TestController()
        { }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("TestController");
        }

    }
}