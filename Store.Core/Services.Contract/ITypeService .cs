using Store.Core.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface ITypeService
    {
        Task<IEnumerable<GetTypeBrandDto>> GetAllTypesAsync();
        Task<GetTypeBrandDto> GetTypeByIdAsync(int id);
        Task<GetTypeBrandDto> AddTypeAsync(TypeBrandDto type);
        Task  UpdateTypeAsync(int id,TypeBrandDto type);
        Task DeleteType(int id);
    }
}
