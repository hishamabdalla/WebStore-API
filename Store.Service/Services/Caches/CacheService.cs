using StackExchange.Redis;
using Store.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Store.Service.Services.Caches
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        private readonly ILogger<CacheService> _logger;
        private readonly bool _redisAvailable;

        public CacheService(IConnectionMultiplexer redis, ILogger<CacheService> logger)
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

        public async Task<string> GetCacheKeyAsync(string cacheKey)
        {
            if (!_redisAvailable)
            {
                _logger.LogWarning("Redis is not available. Cache get operation skipped for key: {CacheKey}", cacheKey);
                _logger.LogWarning("Data will be fetched from the primary data source.");
                return null;
            }

            try
            {
                var cacheResponse = await _database.StringGetAsync(cacheKey);
                if (cacheResponse.IsNullOrEmpty) return null;

                _logger.LogInformation("Data retrieved from cache for key: {CacheKey} successfully", cacheKey);
                return cacheResponse.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cache key: {CacheKey}", cacheKey);
                return null;
            }
        }

        public async Task SetCacheKeyAsync(string cacheKey, object response, TimeSpan expireTime)
        {
            if (!_redisAvailable)
            {
                _logger.LogWarning("Redis is not available. Cache set operation skipped for key: {CacheKey}", cacheKey);
                return;
            }

            if (response is null) return;

            try
            {
                var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await _database.StringSetAsync(cacheKey, JsonSerializer.Serialize(response, option), expireTime);

                _logger.LogInformation("Cache set for key: {CacheKey} Successfully", cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache key: {CacheKey}", cacheKey);
            }
        }
    }
}
