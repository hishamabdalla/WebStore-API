using AutoMapper;
using Store.Core.Dtos.Products;
using Store.Core.Entities;
using Store.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Services.Contract;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Store.Service.Services.Products
{
    public class TypeService:ITypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<GetTypeBrandDto> AddTypeAsync(TypeBrandDto typeDto)
        {
            var type = await _unitOfWork.Repository<ProductType, int>().AddAsync(_mapper.Map<ProductType>(typeDto));
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<GetTypeBrandDto>(type);
        }

        public async Task DeleteType(int id)
        {
            var type = await _unitOfWork.Repository<ProductType, int>().GetAsync(id);
            _unitOfWork.Repository<ProductType, int>().Delete(type);
            await _unitOfWork.CompleteAsync();

        }

        public async Task<IEnumerable<GetTypeBrandDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();

            return (_mapper.Map<IEnumerable<GetTypeBrandDto>>(types));

        }

        public async Task<GetTypeBrandDto> GetTypeByIdAsync(int id)
        {
            var type = await _unitOfWork.Repository<ProductType, int>().GetAsync(id);
            return _mapper.Map<GetTypeBrandDto>(type);

        }

        public async Task UpdateTypeAsync(int id,TypeBrandDto type)
        {

            var existingType = await _unitOfWork.Repository<ProductType, int>().GetAsync(id);
            _mapper.Map(type, existingType);
            _unitOfWork.Repository<ProductType, int>().Update(existingType);
            await _unitOfWork.CompleteAsync();

        }
    }
}
