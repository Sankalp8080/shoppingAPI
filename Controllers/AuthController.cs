using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingAPI.Services;

namespace ShoppingAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly Service _service;

        public AuthController(Service service )
        {
            _service =   service;
        }
        [HttpPost("Auth")]
        public IActionResult Auth([FromBody] LoginModel lg)
        {
            if(lg.Username=="admin" && lg.Password == "admin")
            {
                var token =_service.GenerateToken(lg.Username);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok("You are authorized!");
        }

        public class LoginModel
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }
}
