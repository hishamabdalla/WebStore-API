using Store.Core.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface IBrandService
    {
        Task<IEnumerable<GetTypeBrandDto>> GetAllBrandsAsync();
        Task<GetTypeBrandDto> GetBrandByIdAsync(int id);
        Task<GetTypeBrandDto> AddBrandAsync(TypeBrandDto brand);
        Task  UpdateBrandAsync(int id,TypeBrandDto brand);
        Task DeleteBrand(int id);
    }
}
