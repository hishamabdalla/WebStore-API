﻿using AutoMapper;
using Store.Core;
using Store.Core.Dtos.Products;
using Store.Core.Entities;
using Store.Core.Helper;
using Store.Core.Services.Contract;
using Store.Core.Specifications;
using Store.Core.Specifications.Products;
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

        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec)
        {
            var spec = new ProductSpecifications(productSpec);

            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec);
            var mappedProduct = _mapper.Map<IEnumerable<ProductDto>>(products);
            var countSpec = new ProductWithCountSpecifications(productSpec);
            var count= await _unitOfWork.Repository<Product, int>().GetCountAsync(countSpec);

            return new PaginationResponse<ProductDto>(productSpec.PageSize,productSpec.PageIndex,count, mappedProduct) ;
        }


        public async Task<ProductDto> GetProductById(int id)
        {
            var spec = new ProductSpecifications(id);

            var product = await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec);
            var mappedProduct = _mapper.Map<ProductDto>(product);
            return mappedProduct;

        }



        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()=>
             _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync());


        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()=>
                    _mapper.Map<IEnumerable<TypeBrandDto>> (await _unitOfWork.Repository<ProductType, int>().GetAllAsync());

        public async Task<ProductDto> CreateProductAsync(CreateOrUpdateProductDto productDto)
        {
            var product=await _unitOfWork.Repository<Product, int>().AddAsync(_mapper.Map<Product>(productDto));
            await _unitOfWork.CompleteAsync();
            var mappedProduct = _mapper.Map<ProductDto>(product);
            return mappedProduct;
        }

        public async Task<ProductDto> UpdateProductAsync(int id, CreateOrUpdateProductDto productDto)
        {
            var spec = new ProductSpecifications(id);

            var product = await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec);

            if (product == null)
            {
                throw new KeyNotFoundException($"The product with ID {id} does not exist.");
            }

            _mapper.Map(productDto, product); // Map updated properties from DTO to the entity.

            _unitOfWork.Repository<Product, int>().Update(product);
            await _unitOfWork.CompleteAsync();

            var updatedProduct = _mapper.Map<ProductDto>(product);
            return updatedProduct;
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product, int>().GetAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException($"The product with ID {id} does not exist.");
            }

            _unitOfWork.Repository<Product, int>().Delete(product);
            await _unitOfWork.CompleteAsync();
        }


    }
}
