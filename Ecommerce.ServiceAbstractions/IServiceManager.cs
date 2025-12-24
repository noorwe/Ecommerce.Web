using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstractions
{
    public interface IServiceManager
    {
        public IProductServices ProductServices { get; }
        public IBasketService BasketService { get; }

        public IAuthenticationService AuthenticationService { get; }
    }
}
