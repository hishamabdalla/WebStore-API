using StackExchange.Redis;
using Store.Core.Entities;
using Store.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database=redis.GetDatabase();
        }
        public async Task<UserBasket?> GetBasketAsync(string basketId)
        {
            var basket= await _database.StringGetAsync(basketId);

            return basket.IsNullOrEmpty?null:JsonSerializer.Deserialize<UserBasket>(basket);
        }

        public async Task<UserBasket?> SetBasketAsync(UserBasket basket)
        {
           var createdOrUpdatedBasket=await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if(createdOrUpdatedBasket is false) return null;

            return await GetBasketAsync(basket.Id); 
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
           return await _database.KeyDeleteAsync(basketId);
        }

       
    }
}
