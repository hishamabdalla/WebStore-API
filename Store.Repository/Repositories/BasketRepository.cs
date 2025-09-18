using StackExchange.Redis;
using Store.Core.Entities;
using Store.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Store.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        private readonly ILogger<BasketRepository> _logger;
        private readonly bool _redisAvailable;

        public BasketRepository(IConnectionMultiplexer redis, ILogger<BasketRepository> logger)
        {
            _logger = logger;
            _redisAvailable = redis != null;
            
            if (_redisAvailable)
            {
                try
                {
                    _database = redis.GetDatabase();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to get Redis database");
                    _redisAvailable = false;
                }
            }
        }

        public async Task<UserBasket?> GetBasketAsync(string basketId)
        {
            if (!_redisAvailable)
            {
                _logger.LogWarning("Redis is not available. Basket operation skipped.");
                return null;
            }

            try
            {
                var basket = await _database.StringGetAsync(basketId);
                return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<UserBasket>(basket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting basket from Redis for basketId: {BasketId}", basketId);
                return null;
            }
        }

        public async Task<UserBasket?> SetBasketAsync(UserBasket basket)
        {
            if (!_redisAvailable)
            {
                _logger.LogWarning("Redis is not available. Basket operation skipped.");
                return basket; // Return the basket as if it was saved
            }

            try
            {
                var createdOrUpdatedBasket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

                if (createdOrUpdatedBasket is false) return null;

                return await GetBasketAsync(basket.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting basket in Redis for basketId: {BasketId}", basket.Id);
                return basket; // Return the basket as if it was saved
            }
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            if (!_redisAvailable)
            {
                _logger.LogWarning("Redis is not available. Basket deletion skipped.");
                return true; // Return true as if it was deleted
            }

            try
            {
                return await _database.KeyDeleteAsync(basketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting basket from Redis for basketId: {BasketId}", basketId);
                return true; // Return true as if it was deleted
            }
        }
    }
}
