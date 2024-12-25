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
    public class TypesController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypesController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetTypeBrandDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IEnumerable<GetTypeBrandDto>>> GetAllTypes()
        {
            var brands = await _typeService.GetAllTypesAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTypeBrandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTypeBrandDto>> GetType(int id)
        {
            var brand = await _typeService.GetTypeByIdAsync(id);
            return Ok(brand);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetTypeBrandDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<GetTypeBrandDto>> AddBrand(TypeBrandDto brand)
        {
            var newBrand = await _typeService.AddTypeAsync(brand);
            return CreatedAtAction(nameof(GetType), new { id = newBrand.Id }, newBrand);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> UpdateBrand(int id, TypeBrandDto brand)
        {
            await _typeService.UpdateTypeAsync( id, brand);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> DeleteBrand(int id)
        {
            await _typeService.DeleteType(id);
            return NoContent();
        }
    }
}
