using Microsoft.AspNetCore.Mvc;
using MSOrder.API.DTOs;
using MSOrder.Application.DTOs;
using MSOrder.Application.Services;

namespace MS.Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _orderService.GetAllOrdersAsync();

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult> GetByCustomerId(int customerId)
        {
            var result = await _orderService.GetOrdersByCustomerIdAsync(customerId);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateOrderRequest request)
        {
            // Mapear Request del controller a DTO de Application
            var createDto = new CreateOrderDto
            {
                CustomerId = request.CustomerId,
                OrderItems = request.OrderItems.Select(item => new CreateOrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            var result = await _orderService.CreateOrderAsync(createDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}