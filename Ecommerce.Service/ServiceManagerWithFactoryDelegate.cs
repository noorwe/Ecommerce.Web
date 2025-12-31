using Ecommerce.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class ServiceManagerWithFactoryDelegate(
        Func<IProductServices> ProductFactory,
        Func<IBasketService> BasketFactory,
        Func<IAuthenticationService> AuthenticationFactory,
        Func<IOrderService> OrderFactory,
        Func<IPaymentService> PaymentFactory
        ) : IServiceManager
    {
        public IProductServices ProductServices => ProductFactory.Invoke();

        public IBasketService BasketService => BasketFactory.Invoke();

        public IAuthenticationService AuthenticationService => AuthenticationFactory.Invoke();

        public IOrderService OrderService => OrderFactory.Invoke();

        public IPaymentService PaymentService => PaymentFactory.Invoke();

    }
}
