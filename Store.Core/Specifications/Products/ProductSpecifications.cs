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

        public ProductSpecifications(string? sort, int? brandId, int? typeId,int pageSize, int pageIndex) :base(
            
            p => 
            (!brandId.HasValue || p.BrandId==brandId)
            &&
            (!typeId.HasValue || p.TypeId==typeId))

        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch(sort)
                {
                   
                    case "priceAsc":
                        AddOrderBy(p=>p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                       AddOrderBy(p=>p.Name);
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
            ApplyPagination((pageSize) * (pageIndex - 1), pageSize);
        }

        private void ApplyInclude()
        {
            Include.Add(p => p.Brand);
            Include.Add(p => p.Type);
        }
    }
}
