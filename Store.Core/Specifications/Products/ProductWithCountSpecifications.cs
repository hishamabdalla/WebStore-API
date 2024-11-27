using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.Products
{
    public class ProductWithCountSpecifications:BaseSpecifications<Product,int>
    {
        public ProductWithCountSpecifications(ProductSpecParams productSpec): base( p =>
            (string.IsNullOrEmpty(productSpec.Search) || p.Name.ToLower().Contains(productSpec.Search)
            &&
            (!productSpec.BrandId.HasValue || p.BrandId == productSpec.BrandId)
            &&
            (!productSpec.TypeId.HasValue || p.TypeId == productSpec.TypeId)))
        {


        }
    }
}
