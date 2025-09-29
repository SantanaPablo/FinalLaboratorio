using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSCustomer.API.DTOs;
using MSCustomer.Application.DTOs;
using MSCustomer.Application.Services;

namespace MSCustomer.API.Controllers
{
    //[Authorize] para autorizar con JWT
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _customerService.GetAllCustomersAsync();

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult> GetByEmail(string email)
        {
            var result = await _customerService.GetCustomerByEmailAsync(email);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            // Mapear Request del controller a DTO de Application
            var createDto = new CreateCustomerDto
            {
                Name = request.Name,
                Email = request.Email,
                Address = request.Address
            };

            var result = await _customerService.CreateCustomerAsync(createDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateCustomerRequest request)
        {
            // Mapear Request del controller a DTO de Application
            var updateDto = new UpdateCustomerDto
            {
                Name = request.Name,
                Email = request.Email,
                Address = request.Address
            };

            var result = await _customerService.UpdateCustomerAsync(id, updateDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}