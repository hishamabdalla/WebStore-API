using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Store.Core;
using Store.Core.Dtos.Auth;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contract;
using Store.Service.Services.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.Service.Users
{
    /// <summary>
    /// Provides services for user authentication and registration.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IdentityDbContext _identityDbContext;


        /// <summary>
        /// Initializes a new instance of <see cref="UserService"/>.
        /// </summary>
        /// <param name="userManager">Identity user manager for managing users.</param>
        /// <param name="signInManager">Identity sign-in manager for authentication.</param>
        /// <param name="tokenService">Token service for generating JWT tokens.</param>
        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }


        /// <inheritdoc />
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Validate input
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new LoginResponseDto
                {
                    ErrorMessage = "Invalid login credentials."
                };
            }

            // Verify the password and handle lockout
            var passwordCheckResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);
            if (!passwordCheckResult.Succeeded)
            {
                return new LoginResponseDto
                {
                    ErrorMessage = "Invalid login credentials."
                };
            }

            // Generate the response DTO with user details and token
            var response = await GenerateUserDtoAsync(user);

            return new LoginResponseDto
            {
                DisplayName=response.DisplayName,
                Email=response.Email,
                Token=response.Token
            };
        }

        /// <inheritdoc />
        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
          if(await CheckEmailExits(registerDto.Email)) return null;

            var user = new AppUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email.Split("@")[0],
                PhoneNumber = registerDto.PhoneNumber,
                 
            };
           var result=await  _userManager.CreateAsync(user,registerDto.Password);

            if(!result.Succeeded) return null;

            await _userManager.AddToRoleAsync(user, "Customer");
            return await GenerateUserDtoAsync(user) ;


        }

        /// <summary>
        /// Generates a <see cref="UserDto"/> for a given user, including a JWT token.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user DTO.</returns>
        private async Task<UserDto> GenerateUserDtoAsync(AppUser user)
        {
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName ,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }

        public async Task<bool> CheckEmailExits(string email)
        {
           return await _userManager.FindByEmailAsync(email) is not null;
        }

       
    }
}
