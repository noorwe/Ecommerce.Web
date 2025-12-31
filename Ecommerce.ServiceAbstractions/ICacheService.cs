using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstractions
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string key);

        Task SetAsync(string key, object value, TimeSpan timeToLive);
    }
}
