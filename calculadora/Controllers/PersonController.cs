using Estudo.Model;
using Estudo.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Estudo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {

        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _service;

        public PersonController
        (
            ILogger<PersonController> logger,
            IPersonService service
        )
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = _service.FindById(id);
            if(person == null)
                return NotFound();
            return Ok(person);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            if (person == null)
                return BadRequest();
            return Ok(_service.Create(person));
        }

        [HttpPut]
        public IActionResult Put([FromBody] Person person)
        {
            if (person == null)
                return BadRequest();
            return Ok(_service.Update(person));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}