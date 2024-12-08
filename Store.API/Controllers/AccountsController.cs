using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Core;
using Store.Core.Dtos.Auth;

namespace Store.API.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly IUserService _userService;

        public AccountsController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);

            if(user == null)
            {
                //error
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);

            if (user == null)
            {
                //error
                return BadRequest();
            }

            return Ok(user);
        }
    }
}
