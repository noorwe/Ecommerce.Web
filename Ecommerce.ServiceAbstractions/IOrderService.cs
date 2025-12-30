using Ecommerce.Shared.DTOS.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstractions
{
    public interface IOrderService
    {

        Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email);

        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();

        Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string email);

        Task<OrderToReturnDto> GetOrderAsync(Guid id);
    }
}
