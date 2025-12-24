using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared.DTOS.IdentityDtos;
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
    public class AuthenticationController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
            
            return Ok(user);
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _serviceManager.AuthenticationService.RegisterAsync(registerDto);

            return Ok(user);
        }
    }
}
