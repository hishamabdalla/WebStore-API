﻿using StackExchange.Redis;
using Store.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Service.Services.Caches
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;

        public CacheService(IConnectionMultiplexer redis)
        {
            _database=redis.GetDatabase();
        }
        public async Task<string> GetCacheKeyAsync(string cacheKey)
        {
            var cacheResponse = await _database.StringGetAsync(cacheKey);
            if (cacheResponse.IsNullOrEmpty) return null;
            return cacheResponse.ToString();

        }
        public async Task SetCacheKeyAsync(string cacheKey, object response, TimeSpan expireTime)
        {
            if (response is null) return  ;

            var option=new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await _database.StringSetAsync(cacheKey, JsonSerializer.Serialize(response,option),expireTime);
        }
    }
}