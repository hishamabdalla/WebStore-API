using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Dtos.Auth
{
    public class LoginResponseDto:UserDto
    {
        public string? ErrorMessage {  get; set; }
    }
}
