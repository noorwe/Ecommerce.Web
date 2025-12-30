using AutoMapper;
using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.Domain.Exceptions;
using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared.DTOS.IdentityDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration, IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException(email);
            if (user.Address is not null)
            {
                return _mapper.Map<Address, AddressDto>(user.Address);
            }
            else
            {
                throw new AddressNotFoundException(user.UserName!);

            }
        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto addressDto, string email)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException(email);
            if (user.Address is not null)
            {
                _mapper.Map(addressDto, user.Address);

                
            }
            else
            {
                var newAddress = _mapper.Map<AddressDto, Address>(addressDto);
                user.Address = newAddress;

            }
            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException(email);
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await CreateTokenAsync(user)
            };
        }

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
                    Token = await CreateTokenAsync(user)
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
                    Token = await CreateTokenAsync(appUser)
                };
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);

            }
        }



        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id!),

            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var secretKey = _configuration.GetSection("JwtOptions")["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("JwtOptions")["Issuer"],
                audience: _configuration.GetSection("JwtOptions")["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

