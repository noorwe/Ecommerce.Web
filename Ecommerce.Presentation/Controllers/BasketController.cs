using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared.DTOS.BasketDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager _serviceManager) : ControllerBase
    {
        // Get Basket
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string key)
        {
            var basket = await _serviceManager.BasketService.GetBasketAsync(key);
           
            return Ok(basket);
        }

        // Create Or Update Basket
        [HttpPost("CreateOrUpdateBasket")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basketDto)
        {
            var basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDto);
            return Ok(basketDto);
        }

        // Delete Basket
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string key)
        {
            var result = await _serviceManager.BasketService.DeleteBasketAsync(key);
            return Ok(result);
        }
    }
}
