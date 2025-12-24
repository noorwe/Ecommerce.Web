using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared;
using Ecommerce.Shared.DTOS.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ProductController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        #region Get All Products
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _serviceManager.ProductServices.GetAllProductsAsync(queryParams);
            return Ok(products);
        }

        #endregion

        #region Get Product By Id

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _serviceManager.ProductServices.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        #endregion

        #region Get All Brands

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var brands = await _serviceManager.ProductServices.GetAllBrandsAsync();
            return Ok(brands);
        }
        #endregion

        #region Get All Types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductServices.GetAllTypesAsync();
            return Ok(types);
        }
        #endregion
    }
}
