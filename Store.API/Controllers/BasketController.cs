using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core.Dtos.Basket;
using Store.Core.Entities;
using Store.Core.Repositories.Interfaces;
using Store.Repository.Repositories;

namespace Store.API.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
             _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<UserBasket>> GetBasketAsync(string? id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket == null)
            {
                new UserBasket() { Id = id };
            }
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<UserBasket>> CreateOrUpdateBasket(UserBasketDto userBasketDto)
        {
            var basket = await _basketRepository.SetBasketAsync(_mapper.Map<UserBasket>(userBasketDto));

            if(basket == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(basket);
           
        }

        [HttpDelete]
         public async Task DeleteBasket(string id)
         {
             await _basketRepository.DeleteBasketAsync(id);   
         }
    }
}
