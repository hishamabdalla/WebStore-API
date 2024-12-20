using Store.Core.Entities.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface IEmailService
    {
      Task SendEmail(MailRequest mailRequest);
    }
}
