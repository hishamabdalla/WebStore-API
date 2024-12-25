  using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core;
using Store.Core.Dtos.Orders;
using Store.Core.Entities.Order;
using Store.Core.Services.Contract;
using System.Security.Claims;

namespace Store.API.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public OrdersController(IOrderService orderService,IMapper mapper,IUnitOfWork unitOfWork)
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
           var address= mapper.Map<Address>(model.shipToAddress);
          var order=await  orderService.CreateOrderAsync(userEmail, model.BasketId, model.DeliveryMethodId, address);

            if(order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var orderToReturn=mapper.Map<OrderToReturnDto>(order);
            return Ok(orderToReturn);
        }

        [HttpGet("GetOrdersForSpecificUser")]
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var orders =await  orderService.GetOrdersForSpecificUser(userEmail);
            if (orders is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(mapper.Map<IEnumerable<OrderToReturnDto>>(orders));
        }

        [HttpGet("GetOrderForSpecificUser/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderForSpecificUser(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var order = await orderService.GetOrderByIdForSpecificUser(userEmail,orderId);
            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet("GetDeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        { 
           var deliveryMethods=await unitOfWork.Repository<DeliveryMethod,int>().GetAllAsync();

            if (deliveryMethods is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(deliveryMethods);
        }

        [HttpDelete("{orderId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var success = await orderService.CancelOrderAsync(userEmail, orderId);

            if (!success) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(new { message = "Order cancelled successfully." });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")] // Restrict access to users with Admin role
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await orderService.GetAllOrdersAsync(); // This service method would return all orders

            if (orders == null || !orders.Any()) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(mapper.Map<IEnumerable<OrderToReturnDto>>(orders));
        }


    }
}
