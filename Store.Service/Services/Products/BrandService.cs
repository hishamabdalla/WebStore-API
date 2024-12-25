using AutoMapper;
using Store.Core;
using Store.Core.Dtos.Products;
using Store.Core.Entities;
using Store.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Products
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        
        public async Task<GetTypeBrandDto> AddBrandAsync(TypeBrandDto brandDto)
        {
             var brand= await _unitOfWork.Repository<ProductBrand, int>().AddAsync(_mapper.Map<ProductBrand>(brandDto));
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<GetTypeBrandDto>(brand);
        }

        public async Task DeleteBrand(int id)
        {
            var brand = await  _unitOfWork.Repository<ProductBrand, int>().GetAsync(id);
             _unitOfWork.Repository<ProductBrand, int>().Delete(brand);
            await _unitOfWork.CompleteAsync();
           
        }

        public async Task<IEnumerable<GetTypeBrandDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();

            return (_mapper.Map<IEnumerable<GetTypeBrandDto>>(brands));

        }

        public async Task<GetTypeBrandDto> GetBrandByIdAsync(int id)
        {
            var brand = await _unitOfWork.Repository<ProductBrand, int>().GetAsync(id);
            return _mapper.Map<GetTypeBrandDto>(brand);

        }

        public async Task UpdateBrandAsync(int id,TypeBrandDto brand)
        {

            var existingBrand = await _unitOfWork.Repository<ProductBrand, int>().GetAsync(id);
            _mapper.Map(brand, existingBrand);
            _unitOfWork.Repository<ProductBrand, int>().Update(existingBrand);
            await _unitOfWork.CompleteAsync();
            
        }
    }
}
