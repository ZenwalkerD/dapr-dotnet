using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HiController : ControllerBase
    {
        // POST api/<HiController>
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            Console.WriteLine("Backend API Invoked.... value: " + value);

            return Ok("Message Recieved in Service B, value = " + value);
        }
    }
}
