using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Specification
{
    public class ProductCountSpecification : BaseSpecification<Product, int>
    {
        public ProductCountSpecification(ProductQueryParams queryParams) :
            base(ProductSpecificationHelper.GetProductCriteria(queryParams))
        {

        }
    }
}
