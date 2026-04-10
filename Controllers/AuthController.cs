using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingAPI.Services;
using Infrastructure.Models;
using Infrastructure;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(
    Summary = "Authenticate user",
    Description = "Validates user credentials and returns a JWT token for authentication",
    Tags = new[] { "Authentication" }
        )]
        [SwaggerResponse(200, "Authentication successful", typeof(object))]
        [SwaggerResponse(401, "Invalid credentials")]
        [HttpPost("Auth")]
        public async Task<IActionResult> Auth([FromBody] IUserIM lg)
        {
            int result = 0;
            result = await _userLoginRegister.CheckUserLogin(lg.username, lg.password);
            if (result > 0)
            {
                var token = _service.GenerateToken(lg.username);
                return Ok(new { token });
            }
            return Unauthorized();
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterIM reg)
        {
            int result = 0;
            result = await _userLoginRegister.UserRegister(reg.username, reg.password, reg.email, reg.phoneno, reg.dob);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("GetUserProfile")]
        public async Task <IActionResult> GetUserProfile([FromBody] IUserIM im)
        {
            var result = await _userLoginRegister.GetUserDetails(im.username, im.password);
            return Ok(result);
        }

    }
}
