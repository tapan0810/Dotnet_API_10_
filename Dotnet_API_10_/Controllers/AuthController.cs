using Dotnet_API_10_.Dtos;
using Dotnet_API_10_.Entities;
using Dotnet_API_10_.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_API_10_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        public static User user = new User();

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(USerDto request)
        {
            var user = await authService.RegisterAsync(request);

            if (user is null)
                return BadRequest("User already exists!");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(USerDto request)
        {
            var token = await authService.LoginAsync(request);
            if (token is null)
                return BadRequest("Invalid User");
            return Ok(token);
        }
    }
}
