using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Estudo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {

        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return BadRequest("Invalid Input");
        }

    }
}
