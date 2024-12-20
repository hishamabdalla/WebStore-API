using Store.Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core
{
    public interface IUserService
    {
       Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
       Task<string> RegisterAsync(RegisterDto registerDto);
       Task<bool> CheckEmailExits(string email);


    }
}
