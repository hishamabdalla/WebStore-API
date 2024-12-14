using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<UserBasket?> GetBasketAsync(string basketId);
        Task<UserBasket?> SetBasketAsync(UserBasket basket);
        Task<bool> DeleteBasketAsync(string basketId);


    }
}
