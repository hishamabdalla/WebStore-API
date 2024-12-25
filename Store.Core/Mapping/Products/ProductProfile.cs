using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Store.Core.Dtos.Products;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Mapping.Products
{
    public class ProductProfile :Profile 
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.BrandName, option => option.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.TypeName, option => option.MapFrom(s => s.Type.Name))
                //----> First Way
               // .ForMember(d => d.PictureUrl, option => option.MapFrom(s => $"{configuration["BaseUrl"]}{s.PictureUrl}"));
                //----> Second Way
                .ForMember(d => d.PictureUrl, option => option.MapFrom(new PictureUrlResolver(configuration)));

            CreateMap<ProductType,TypeBrandDto>().ReverseMap();
            CreateMap<ProductBrand,TypeBrandDto>().ReverseMap();
            CreateMap<ProductBrand,GetTypeBrandDto>().ReverseMap();
            CreateMap<ProductType, GetTypeBrandDto>();

            CreateMap<CreateOrUpdateProductDto, Product>().ReverseMap();
        }
    }
}
