using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Attributes;
using Store.API.Errors;
using Store.Core.Dtos.Products;
using Store.Core.Helper;
using Store.Core.Services.Contract;
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
        [Cached(1)]
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec )
        {
            var result= await _productService.GetAllProductsAsync(productSpec);
            return Ok(result); 
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost] // POST : BaseUrl/api/Products
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateOrUpdateProductDto newProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid product data"));

            var createdProduct = await _productService.CreateProductAsync(newProduct);
            

            if (createdProduct == null)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Product could not be created"));

            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);

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



        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")] // PUT: BaseUrl/api/Products/{id}
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ProductDto>> UpdateProductAsync(int id, [FromBody] CreateOrUpdateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid product data"));

            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, $"An error occurred: {ex.Message}"));
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] // DELETE: BaseUrl/api/Products/{id}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent(); // HTTP 204: No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, $"An error occurred: {ex.Message}"));
            }
        }


    }
}
