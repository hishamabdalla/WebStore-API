using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.Products
{
    public class ProductSpecifications:BaseSpecifications<Product,int>
    {

        public ProductSpecifications(int id) :base(p=>p.Id==id)
        {
            ApplyInclude();

        }

        public ProductSpecifications(ProductSpecParams productSpec) : base(
            p =>
            (!productSpec.BrandId.HasValue || p.BrandId == productSpec.BrandId)
            &&
            (!productSpec.TypeId.HasValue || p.TypeId == productSpec.TypeId))
        {
            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {

                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;

                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }

            ApplyInclude();

            //300
            //PageSize=50
            //PageIndex=3
            ApplyPagination((productSpec.PageSize) * (productSpec.PageIndex - 1), productSpec.PageSize);
        }

        private void ApplyInclude()
        {
            Include.Add(p => p.Brand);
            Include.Add(p => p.Type);
        }
    }
}
