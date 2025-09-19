using MSProduct.Domain;
namespace MSProduct.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        Task<Result<IEnumerable<Product>>> GetAllAsync();
        Task<Result<Product>> GetByIdAsync(int id);
        Task<Result<Product>> AddAsync(Product product);
        Task<Result<Product>> UpdateAsync(Product product);
        Task<Result> DeleteAsync(int id);
        Task<Result> UpdateStockAsync(int productId, int quantity);
    }
}
