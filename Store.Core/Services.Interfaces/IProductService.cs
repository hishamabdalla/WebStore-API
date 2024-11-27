using Store.Core.Dtos.Products;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(string? sort, int? brandId, int? typeId);
        Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();
        Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();

        Task<ProductDto> GetProductById(int id);

    }
}
