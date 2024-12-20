using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities.Email
{
    public class MailRequest
    {
        public string MailTo {  get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<IFormFile>? attachment { get; set; }=null;

    }
}
