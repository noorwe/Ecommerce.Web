using AutoMapper;
using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Shared.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBranda.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductTypes.Name));

        }

    }
}
