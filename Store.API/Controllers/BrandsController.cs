using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core.Dtos.Products;
using Store.Core.Services.Contract;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetTypeBrandDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IEnumerable<GetTypeBrandDto>>> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTypeBrandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<GetTypeBrandDto>> GetBrand(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            return Ok(brand);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetTypeBrandDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetTypeBrandDto>> AddBrand(TypeBrandDto brand)
        {
            var newBrand = await _brandService.AddBrandAsync(brand);
            return CreatedAtAction(nameof(GetBrand), new { id = newBrand.Id }, newBrand);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBrand(int id, TypeBrandDto brand)
        {
            await _brandService.UpdateBrandAsync(id,brand);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            await _brandService.DeleteBrand(id);
            return NoContent();
        }
    }
}
