using Ecommerce.Domain.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Specification.OrderModuleSpecifications
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        // Get All Orders By Email
        public OrderSpecification(string email):base(o => o.UserEmail == email)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderBy(o => o.OrderDate);
        }
        // Get Specific Order By Id 
        public OrderSpecification(Guid id) : base(o => o.Id == id)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
        }

    }
}
