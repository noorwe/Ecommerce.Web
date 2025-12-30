using Ecommerce.Shared.DTOS.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstractions
{
    public interface IAuthenticationService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        // Check Email Exists
        Task<bool> CheckEmailAsync(string email);

        // Get Current User Address
        Task<AddressDto> GetCurrentUserAddressAsync(string email);

        // Update Current User Address
        Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto addressDto, string email);

        // Get Current User
        Task<UserDto> GetCurrentUserAsync(string email);
    }
}
