using Microsoft.AspNetCore.Mvc;
using MSProduct.API.DTOs;
using MSProduct.Application.DTOs;
using MSProduct.Application.Services;

namespace MS.Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _productService.GetAllProductsAsync();

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateProductRequest request)
        {
            // Mapear Request del controller a DTO de Application
            var createDto = new CreateProductDto
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            var result = await _productService.CreateProductAsync(createDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateProductRequest request)
        {
            // Mapear Request del controller a DTO de Application
            var updateDto = new UpdateProductDto
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            var result = await _productService.UpdateProductAsync(id, updateDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpPost("{id}/update-stock")]
        public async Task<ActionResult> UpdateStock(int id, [FromBody] UpdateStockRequest request)
        {
            var result = await _productService.UpdateStockAsync(id, request.Quantity);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok("Stock actualizado correctamente");
        }
    }
}