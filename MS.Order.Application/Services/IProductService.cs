using MSOrder.Application.DTOs;

namespace MSOrder.Application.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<bool> UpdateStockAsync(int productId, int quantity);
        Task<bool> CheckStockAvailabilityAsync(int productId, int quantity);
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}