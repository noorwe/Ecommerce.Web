using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared.DTOS.IdentityDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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


        [HttpGet("checkemail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var exists = await _serviceManager.AuthenticationService.CheckEmailAsync(email);
            return Ok(exists);
        }


        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _serviceManager.AuthenticationService.GetCurrentUserAsync(email!);
            return Ok(user);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManager.AuthenticationService.GetCurrentUserAddressAsync(email!);
            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var updatedAddress = await _serviceManager.AuthenticationService.UpdateCurrentUserAddressAsync(addressDto, email!);
            return Ok(updatedAddress);
        }
    }
}
