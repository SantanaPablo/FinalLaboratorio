using MSOrder.Domain;

namespace MSOrder.Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Task<Result<IEnumerable<Order>>> GetAllAsync();
        Task<Result<Order>> GetByIdAsync(int id);
        Task<Result<IEnumerable<Order>>> GetByCustomerIdAsync(int customerId);
        Task<Result<Order>> AddAsync(Order order);
        Task<Result> DeleteAsync(int id);
    }
}