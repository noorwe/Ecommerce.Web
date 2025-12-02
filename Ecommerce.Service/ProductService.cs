using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Service.Specification;
using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared;
using Ecommerce.Shared.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class ProductService : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDto>>(brands);

        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(queryParams);
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(Spec);
            var DataToReturn = _mapper.Map<IEnumerable<ProductDto>>(products);
            var CountOfReturnedData = DataToReturn.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var CountOfProducts = await _unitOfWork.GetRepository<Product, int>().CountAsync(CountSpec);
            return new PaginatedResult<ProductDto>(queryParams.PageIndex, CountOfReturnedData, CountOfProducts, DataToReturn );
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDto>>(types);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await  _unitOfWork.GetRepository<Product, int>().GetByIdAsync(Spec);
            return  _mapper.Map<ProductDto?>(product);
        }
    }
}
