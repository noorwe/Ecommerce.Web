using Ecommerce.Domain.Contracts;
using Ecommerce.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class CacheService(ICacheRepository _cacheRepository) : ICacheService
    {
        public async Task<string?> GetAsync(string key)
        {
            return await _cacheRepository.GetAsync(key);
        }

        public async Task SetAsync(string key, object value, TimeSpan timeToLive)
        {
            var stringValue = JsonSerializer.Serialize(value);
            await _cacheRepository.SetAsync(key, stringValue, timeToLive);
        }
    }
}
