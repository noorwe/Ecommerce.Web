using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared.DTOS.OrderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    public class OrderController(IServiceManager _serviceManager) : ApiBaseController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderDto orderDto)
        {
            
            var order = await _serviceManager.OrderService.CreateOrderAsync(orderDto, GetEmailFromToken());
            return Ok(order);
        }

        // Get Orders for User By Email

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersForUser()
        {
            var orders = await _serviceManager.OrderService.GetOrdersForUserAsync(GetEmailFromToken());
            return Ok(orders);
        }

        // Get Order By Id for User
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderByIdForUser(Guid id)
        {
            var order = await _serviceManager.OrderService.GetOrderAsync(id);
            return Ok(order);
        }

        [Authorize]
        [HttpGet("deliverymethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }

    }
}
