using Microsoft.EntityFrameworkCore;
using MSOrder.Domain;
using MSOrder.Infrastructure.Data;
using MSOrder.Infrastructure.Repositories;

namespace MSOrder.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Order>>> GetAllAsync()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
                return Result.Success<IEnumerable<Order>>(orders);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<Order>>(ex.Message);
            }
        }

        public async Task<Result<Order>> GetByIdAsync(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == id);

                return order != null
                    ? Result.Success(order)
                    : Result.Fail<Order>("Orden no encontrada");
            }
            catch (Exception ex)
            {
                return Result.Fail<Order>(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Order>>> GetByCustomerIdAsync(int customerId)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.CustomerId == customerId)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                return Result.Success<IEnumerable<Order>>(orders);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<Order>>(ex.Message);
            }
        }

        public async Task<Result<Order>> AddAsync(Order order)
        {
            try
            {
                order.OrderDate = DateTime.UtcNow;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Recargar la orden con los items para devolver el objeto completo
                var savedOrder = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == order.Id);

                return Result.Success(savedOrder!);
            }
            catch (Exception ex)
            {
                return Result.Fail<Order>(ex.Message);
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                    return Result.Fail("Orden no encontrada");

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}