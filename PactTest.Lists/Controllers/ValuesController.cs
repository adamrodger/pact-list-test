using Microsoft.AspNetCore.Mvc;

namespace PactTest.Lists.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok(new { values = new[] { "value1", "value2" } });
        }
    }
}
