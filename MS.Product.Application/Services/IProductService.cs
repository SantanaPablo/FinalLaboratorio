using MSProduct.Application.DTOs;
using MSProduct.Domain;

namespace MSProduct.Application.Services
{
    public interface IProductService
    {
        Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<Result<ProductDto>> GetProductByIdAsync(int id);
        Task<Result<ProductDto>> CreateProductAsync(CreateProductDto createDto);
        Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto updateDto);
        Task<Result> DeleteProductAsync(int id);
        Task<Result> UpdateStockAsync(int productId, int quantity);
    }
}