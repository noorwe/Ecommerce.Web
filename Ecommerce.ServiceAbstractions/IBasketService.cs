using Ecommerce.Shared.DTOS.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstractions
{
    public interface IBasketService
    {
        Task<BasketDto> GetBasketAsync(string key);
        Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket);
        Task<bool> DeleteBasketAsync(string key);
    }
}
