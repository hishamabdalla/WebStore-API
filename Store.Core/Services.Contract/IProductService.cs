using Store.Core.Dtos.Products;
using Store.Core.Entities;
using Store.Core.Helper;
using Store.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec);
        Task<ProductDto> GetProductById(int id);
        Task<ProductDto> CreateProductAsync(CreateOrUpdateProductDto product);
        Task<ProductDto> UpdateProductAsync(int id, CreateOrUpdateProductDto productDto);
        Task DeleteProductAsync(int id);

        Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();
        Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();

    }
}
