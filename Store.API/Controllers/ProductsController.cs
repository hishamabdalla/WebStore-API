using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Attributes;
using Store.API.Errors;
using Store.Core.Dtos.Products;
using Store.Core.Helper;
using Store.Core.Services.Interfaces;
using Store.Core.Specifications.Products;

namespace Store.API.Controllers
{
   // [Authorize]
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [ProducesResponseType(typeof(PaginationResponse<ProductDto>),StatusCodes.Status200OK)]
        [HttpGet] // Get : BaseUrl/api/Products
        [Cached(100)]
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec )
        {
            var result= await _productService.GetAllProductsAsync(productSpec);
            return Ok(result); 
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]

        [HttpGet("brands")] // Get : BaseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();
            return Ok(result);
        }
        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]

        [HttpGet("types")] // Get : BaseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();
            return Ok(result);
        }
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]

        [HttpGet("{id}")] // Get : BaseUrl/api/Products/{id}
        public async Task<ActionResult<ProductDto>> GetProductById(int? id)
        {
            if (id == null)
              return  BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var result = await _productService.GetProductById(id.Value);

            if (result == null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound,$"The Product With Id: {id} Not Found"));

            return Ok(result);
        }

    }
}
