using Ecommerce.Shared;
using Ecommerce.Shared.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstractions
{
    public interface IProductServices
    {
        Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams);

        Task<ProductDto?> GetProductByIdAsync(int id);

        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();

        Task<IEnumerable<TypeDto>> GetAllTypesAsync();



    }
}
