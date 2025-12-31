using Ecommerce.Domain.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Specification.OrderModuleSpecifications
{
    public class OrderWithPaymentIntentIdSpecification : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntentIdSpecification(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
