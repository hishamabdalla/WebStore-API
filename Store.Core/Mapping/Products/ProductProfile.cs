using AutoMapper;
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
        public ProductProfile()
        {
            CreateMap<Product,ProductDto>()
                .ForMember(d=>d.BrandName,option=>option.MapFrom(s=>s.Brand.Name))
                .ForMember(d=>d.TypeName,option=>option.MapFrom(s=>s.Type.Name));

            CreateMap<ProductType,TypeBrandDto>();
            CreateMap<ProductBrand,TypeBrandDto>();

        }
    }
}
