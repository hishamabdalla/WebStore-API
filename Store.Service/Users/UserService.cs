using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Store.Core;
using Store.Core.Dtos.Auth;
using Store.Core.Entities.Email;
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
        private readonly IEmailService _emailService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;




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
            IMapper mapper,
            IEmailService emailService,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailService = emailService;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
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
            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordCheckResult)
            {
                return new LoginResponseDto
                {
                    ErrorMessage = "Invalid login credentials."
                };
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponseDto
                {
                    ErrorMessage = "Email is not confirmed"
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
        public async Task<string> RegisterAsync(RegisterDto registerDto)
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
            //await SendOtpMailAsync(registerDto.Email);
            var OTP= _userManager.GenerateEmailConfirmationTokenAsync(user);

            return await OTP ;


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
       
        private async Task SendOtpMailAsync(string email)
        {
            var mailrequest = new MailRequest();
            mailrequest.MailTo = email;
            mailrequest.Subject = "Thanks for registering : OTP";
            var user = await _userManager.FindByEmailAsync(email);
            var OtpText = _userManager.GenerateEmailConfirmationTokenAsync(user);
            mailrequest.Body = GenerateEmailBody(user.DisplayName,await OtpText);
            await this._emailService.SendEmail(mailrequest);


        }

        public async Task<bool> ForgetPasswordAsync(ForgetPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(token))
                return false;

            // Prepare a plain text or message-based body instead of a clickable link
            var mailRequest = new MailRequest
            {
                MailTo = user.Email,
                Subject = "Reset Password",
                Body = $"Hello {user.DisplayName},<br><br>" +
                       $"We received a request to reset your password. Please use the following token to reset your password: <strong>{token}</strong>.<br><br>" +
                       "If you did not request this, please ignore this email or contact support."
            };

            // Send the email
            await _emailService.SendEmail(mailRequest);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            return result.Succeeded;
        }

        private string GenerateEmailBody(string name, string otptext)
        {
            string emailbody = "";
            emailbody += "<div style='width:100%; background-color:#f5f5f5; padding:20px; font-family:Arial, sans-serif;'>";
            emailbody += "  <div style='max-width:600px; margin:0 auto; background-color:#ffffff; border:1px solid #ddd; border-radius:10px; overflow:hidden;'>";
            emailbody += "    <div style='background-color:#4CAF50; color:#ffffff; padding:15px; text-align:center;'>";
            emailbody += "      <h1 style='margin:0;'>Welcome to Our Service</h1>";
            emailbody += "    </div>";
            emailbody += "    <div style='padding:20px; line-height:1.6; color:#333;'>";
            emailbody += "      <h2 style='margin:0 0 10px;'>Hello " + name + ",</h2>";
            emailbody += "      <p>Thank you for registering with us. Please use the OTP below to complete your registration process:</p>";
            emailbody += "      <div style='text-align:center; margin:20px 0;'>";
            emailbody += "        <span style='display:inline-block; background-color:#f0f0f0; padding:10px 20px; font-size:24px; border:1px solid #ddd; border-radius:5px; letter-spacing:2px;'>" + otptext + "</span>";
            emailbody += "      </div>";
            emailbody += "      <p style='margin:20px 0 0;'>If you did not request this OTP, please ignore this email or contact support if you have any concerns.</p>";
            emailbody += "    </div>";
            emailbody += "    <div style='background-color:#f5f5f5; padding:10px; text-align:center; color:#666; font-size:12px;'>";
            emailbody += "      <p style='margin:0;'>This is an automated message. Please do not reply.</p>";
            emailbody += "      <p style='margin:5px 0;'>Contact Support: <a href='mailto:support@ourservice.com' style='color:#4CAF50;'>support@ourservice.com</a></p>";
            emailbody += "    </div>";
            emailbody += "  </div>";
            emailbody += "</div>";

            return emailbody;
        }




    }
}
