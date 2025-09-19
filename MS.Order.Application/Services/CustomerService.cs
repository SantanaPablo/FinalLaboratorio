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
                _logger.LogInformation($"Calling Customer service at {customerServiceUrl} for ID: {customerId}");

                var response = await _httpClient.GetAsync($"{customerServiceUrl}/api/customer/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var customer = JsonSerializer.Deserialize<CustomerDto>(json, options);
                    _logger.LogInformation($"Successfully retrieved customer: {customer?.Name}");
                    return customer;
                }

                _logger.LogWarning($"Customer service returned {response.StatusCode} for customer ID: {customerId}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"HTTP error when calling Customer service for ID: {customerId}");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, $"Timeout when calling Customer service for ID: {customerId}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error when calling Customer service for ID: {customerId}");
                return null;
            }
        }
    }
}