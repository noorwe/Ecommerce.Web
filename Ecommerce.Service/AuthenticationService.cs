using AutoMapper;
using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.Domain.Exceptions;
using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared.DTOS.IdentityDtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager) : IAuthenticationService
    {
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email) ?? throw new UserNotFoundException(loginDto.Email);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (isPasswordValid) 
            {
                return new UserDto
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = CreateTokenAsync()
                };
            }
            else
            {
                throw new UnauthorizedException();
            }
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var appUser = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName,
            };
            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (result.Succeeded)
            {
                return new UserDto()
                {
                    Email = appUser.Email,
                    DisplayName = appUser.DisplayName,
                    Token = CreateTokenAsync()
                };
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);

            }
        }

        private static string CreateTokenAsync()
        {
            return "ThisIs";
        }
    }
}
