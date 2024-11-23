using AutoMapper;
using Store.Core;
using Store.Core.Dtos.Products;
using Store.Core.Entities;
using Store.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()=>       
             _mapper.Map<IEnumerable<ProductDto>>( await _unitOfWork.Repository<Product, int>().GetAllAsync());

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _unitOfWork.Repository<Product, int>().GetAsync(id);
            var mappedProduct = _mapper.Map<ProductDto>(product);
            return mappedProduct;

        }
            


        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()=>
             _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync());


        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()=>
                    _mapper.Map<IEnumerable<TypeBrandDto>> (await _unitOfWork.Repository<ProductType, int>().GetAllAsync());
        

        
    }
}
