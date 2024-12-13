using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.API.Extensions;
using Store.Core;
using Store.Core.Dtos.Auth;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contract;
using System.Security.Claims;

namespace Store.API.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;


        public AccountsController(IUserService userService, UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
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

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
           var userEmail= User.FindFirstValue(ClaimTypes.Email);
            //error
            if (userEmail == null) return BadRequest();

            var user =await _userManager.FindByEmailAsync(userEmail);

            if(user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));  

            return Ok(  new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
           
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
           
            var user = await _userManager.FindByEmailWithAddress(User);

            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }

      

    }
}
