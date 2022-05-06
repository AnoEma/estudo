using Calculadora.Data.VO;
using Calculadora.Service;
using Microsoft.AspNetCore.Mvc;

namespace Calculadora.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _service;

        public AuthController(ILoginService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("")]
        public IActionResult Signin([FromBody] UserVO user)
        {
            if (user == null) return BadRequest("Invalid client request");

            var token = _service.ValidateCredentials(user);
            if (token == null) return Unauthorized();

            return Ok(token);
        }
    }
}