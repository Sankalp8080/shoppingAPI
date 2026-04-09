using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingAPI.Services;
using Infrastructure.Models;
using Infrastructure;
namespace ShoppingAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly Service _service;
        private readonly IUserLoginRegister _userLoginRegister;

        public AuthController(Service service, IUserLoginRegister iu)
        {
            _service = service;
            _userLoginRegister = iu;
        }
        [HttpPost("Auth")]
        public async Task<IActionResult> Auth([FromBody] IUserIM lg)
        {
            int result = 0;
            result = await  _userLoginRegister.CheckUserLogin(lg.username, lg.password);
            if (result >0)
            {
                var token = _service.GenerateToken(lg.username);
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
