using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Services.Interfaces;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet] // Get : BaseUrl/api/Products
        public async Task<IActionResult> GetAllProducts([FromQuery]string? sort, [FromQuery] int?brandId, [FromQuery] int? typeId)
        {
            var result= await _productService.GetAllProductsAsync(sort,brandId,typeId);
            return Ok(result); 
        }

        [HttpGet("brands")] // Get : BaseUrl/api/Products/brands
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();
            return Ok(result);
        }

        [HttpGet("types")] // Get : BaseUrl/api/Products/types
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")] // Get : BaseUrl/api/Products/{id}
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id == null)
              return  BadRequest("Invalid Id !!!!!");

            var result = await _productService.GetProductById(id.Value);

            if (result == null)
                return NotFound($"The Product With Id: {id} Not Found");

            return Ok(result);
        }

    }
}
