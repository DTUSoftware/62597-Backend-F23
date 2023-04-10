using Microsoft.AspNetCore.Mvc;
using ShopBackend.Dtos;
using ShopBackend.Security;

namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/login
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.AuthenticateUser(loginDto);

            if (!result)
            {
                return Unauthorized("Incorrect username or password!");
            }

            var token = _authService.CreateToken();

            return Accepted( new { Token = token });
        }
    }
}
