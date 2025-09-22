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
                _logger.LogInformation($"Llamando al servicio de productos en {productServiceUrl} para el ID: {productId}");

                var response = await _httpClient.GetAsync($"{productServiceUrl}/api/product/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var product = JsonSerializer.Deserialize<ProductDto>(json, options);
                    _logger.LogInformation($"Producto recuperado con éxito: {product?.Name}");
                    return product;
                }

                _logger.LogWarning($"El servicio de productos devolvió {response.StatusCode} para el ID de producto: {productId}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Error HTTP al llamar al servicio de productos para el ID: {productId}");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, $"Tiempo de espera agotado al llamar al servicio de productos para el ID: {productId}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al llamar al servicio de productos para el ID: {productId}");
                return null;
            }
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            try
            {
                var productServiceUrl = _configuration["ServiceUrls:ProductService"];
                _logger.LogInformation($"Actualizando stock en {productServiceUrl} para el producto {productId}, cantidad: {quantity}");

                var requestBody = new { Quantity = quantity };
                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{productServiceUrl}/api/product/{productId}/update-stock", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Stock actualizado con éxito para el producto {productId}");
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning($"No se pudo actualizar el stock. El servicio de productos devolvió {response.StatusCode}: {errorContent}");
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Error HTTP al actualizar el stock para el ID de producto: {productId}");
                return false;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, $"Tiempo de espera agotado al actualizar el stock para el ID de producto: {productId}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar el stock para el ID de producto: {productId}");
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
                _logger.LogError(ex, $"Error al verificar el stock para el ID de producto: {productId}");
                return false;
            }
        }
    }
}