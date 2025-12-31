using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.ServiceAbstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository, UserManager<ApplicationUser> _userManager, IConfiguration _configuration ) 
      //  : IServiceManager
    {
        private readonly Lazy<IProductServices> _lazyProductServices
            = new Lazy<IProductServices>(() => new ProductService(_unitOfWork, _mapper));
        public IProductServices ProductServices => _lazyProductServices.Value;

        private readonly Lazy<IBasketService> _lazyBasketService
            = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));
        public IBasketService BasketService => _lazyBasketService.Value;

        private readonly Lazy<IAuthenticationService> _lazyAuthenticationService
            = new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager, _configuration, _mapper));
        public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;

        private readonly Lazy<IOrderService> _lazyOrderService
    = new Lazy<IOrderService>(() => new OrderService(_mapper, _basketRepository, _unitOfWork));
        public IOrderService OrderService => _lazyOrderService.Value;
    }
}
