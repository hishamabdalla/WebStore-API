﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountsController(IUserService userService,UserManager<AppUser> userManager,ITokenService tokenService)
        {
            _userService = userService;
           _userManager = userManager;
            _tokenService = tokenService;
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

        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
           var userEmail= User.FindFirstValue(ClaimTypes.Email);
            //error
            if (userEmail == null) return BadRequest();

            var user =await _userManager.FindByEmailAsync(userEmail);

            if(user == null) return BadRequest();  

            return Ok(  new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
           
        }
    }
}
