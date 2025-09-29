using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using MSCustomer.API.DTOs;
using MSCustomer.Application.Services;
using MSCustomer.Infrastructure;


namespace MSCustomer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            ICustomerService customerService,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _customerService = customerService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Login con solo el email del customer (sin password, solo para pruebas de JWT)
        ///Luego se puede proteger endpoints con [Authorize]
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            // Validación básica
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                _logger.LogWarning("Login failed: Email is required");
                return BadRequest(new { Error = "El email es requerido" });
            }

            // Buscar customer por email
            var customerResult = await _customerService.GetCustomerByEmailAsync(request.Email);

            if (!customerResult.IsSuccess)
            {
                _logger.LogWarning("Login failed: Customer not found with email: {Email}", request.Email);
                return NotFound(new { Error = "No se encontró un cliente con ese email" });
            }

            var customer = customerResult.Value;

            // Generar token JWT
            var secretKey = _configuration["JwtSettings:SecretKey"];

            if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
            {
                _logger.LogError("JWT SecretKey is not configured properly");
                return StatusCode(500, new { Error = "Error de configuración del servidor" });
            }

            var token = JwtHelper.GenerateJwtToken(
                customer.Id,
                customer.Email,
                "Customer",
                secretKey
            );

            var response = new LoginResponse
            {
                Token = token,
                CustomerId = customer.Id,
                Email = customer.Email,
                Name = customer.Name,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };

            _logger.LogInformation("Login successful for customer: {CustomerId} - {Email}", customer.Id, customer.Email);

            return Ok(response);
        }

        

    }
}