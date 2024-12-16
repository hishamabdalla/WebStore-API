using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<UserBasket> SetPaymentIntentIdAsync(string basketId);

    }
}
