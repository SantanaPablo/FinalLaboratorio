using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MSOrder.Application.DTOs;
using System.Text;
using System.Text.Json;

namespace MSOrder.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductService> _logger;

        public ProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            try
            {
                var productServiceUrl = _configuration["ServiceUrls:ProductService"];
                _logger.LogInformation($"Calling Product service at {productServiceUrl} for ID: {productId}");

                var response = await _httpClient.GetAsync($"{productServiceUrl}/api/product/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var product = JsonSerializer.Deserialize<ProductDto>(json, options);
                    _logger.LogInformation($"Successfully retrieved product: {product?.Name}");
                    return product;
                }

                _logger.LogWarning($"Product service returned {response.StatusCode} for product ID: {productId}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"HTTP error when calling Product service for ID: {productId}");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, $"Timeout when calling Product service for ID: {productId}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error when calling Product service for ID: {productId}");
                return null;
            }
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            try
            {
                var productServiceUrl = _configuration["ServiceUrls:ProductService"];
                _logger.LogInformation($"Updating stock at {productServiceUrl} for product {productId}, quantity: {quantity}");

                var requestBody = new { Quantity = quantity };
                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{productServiceUrl}/api/product/{productId}/update-stock", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Stock updated successfully for product {productId}");
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning($"Failed to update stock. Product service returned {response.StatusCode}: {errorContent}");
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"HTTP error when updating stock for product ID: {productId}");
                return false;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, $"Timeout when updating stock for product ID: {productId}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error when updating stock for product ID: {productId}");
                return false;
            }
        }

        public async Task<bool> CheckStockAvailabilityAsync(int productId, int quantity)
        {
            try
            {
                var product = await GetProductByIdAsync(productId);
                return product != null && product.StockQuantity >= quantity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when checking stock for product ID: {productId}");
                return false;
            }
        }
    }
}