using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MSOrder.Application.DTOs;
using System.Text.Json;

namespace MSOrder.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(HttpClient httpClient, IConfiguration configuration, ILogger<CustomerService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                var customerServiceUrl = _configuration["ServiceUrls:CustomerService"];
                _logger.LogInformation($"Llamando al servicio de clientes en {customerServiceUrl} para el ID: {customerId}");

                var response = await _httpClient.GetAsync($"{customerServiceUrl}/api/customer/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var customer = JsonSerializer.Deserialize<CustomerDto>(json, options);
                    _logger.LogInformation($"Cliente recuperado con éxito: {customer?.Name}");
                    return customer;
                }

                _logger.LogWarning($"El servicio de clientes devolvió {response.StatusCode} para el ID de cliente: {customerId}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Error HTTP al llamar al servicio de clientes para el ID: {customerId}");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, $"Tiempo de espera agotado al llamar al servicio de clientes para el ID: {customerId}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al llamar al servicio de clientes para el ID: {customerId}");
                return null;
            }
        }
    }
}