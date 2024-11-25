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

        public ProductSpecifications()
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(p => p.Brand);
            Include.Add(p => p.Type);
        }
    }
}
