using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Specification
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product, int>
    {
        public ProductWithBrandAndTypeSpecification(ProductQueryParams queryParams) : 
            base(ProductSpecificationHelper.GetProductCriteria(queryParams))
        {
            AddInclude(p => p.ProductBranda);
            AddInclude(p => p.ProductTypes);

           
            switch (queryParams.Sort)
                {
                    case ProductSortingOptions.PriceAsc:
                        AddOrderBy(p => p.Price);
                        break;
                    case ProductSortingOptions.PriceDesc:
                        AddOrderByDescending(p => p.Price);
                        break;
                    case ProductSortingOptions.NameAsc:
                        AddOrderBy(p => p.Name);
                        break;
                    case ProductSortingOptions.NameDesc:
                        AddOrderByDescending(p => p.Name);
                        break;
                    default:
                        AddOrderBy(p => p.Id);
                        break;
                }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);


        }

        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            AddInclude(p => p.ProductBranda);
            AddInclude(p => p.ProductTypes);
        }
    }
}
