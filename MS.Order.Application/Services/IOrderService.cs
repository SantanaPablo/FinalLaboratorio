using MSOrder.Application.DTOs;
using MSOrder.Domain;

namespace MSOrder.Application.Services
{
    public interface IOrderService
    {
        Task<Result<IEnumerable<OrderDto>>> GetAllOrdersAsync();
        Task<Result<OrderDto>> GetOrderByIdAsync(int id);
        Task<Result<IEnumerable<OrderDto>>> GetOrdersByCustomerIdAsync(int customerId);
        Task<Result<OrderDto>> CreateOrderAsync(CreateOrderDto createDto);
        Task<Result> DeleteOrderAsync(int id);
    }
}