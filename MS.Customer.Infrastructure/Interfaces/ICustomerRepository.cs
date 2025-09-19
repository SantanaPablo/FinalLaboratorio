using MSCustomer.Domain;

namespace MSCustomer.Infrastructure.Repositories
{
    public interface ICustomerRepository
    {
        Task<Result<IEnumerable<Customer>>> GetAllAsync();
        Task<Result<Customer>> GetByIdAsync(int id);
        Task<Result<Customer>> GetByEmailAsync(string email);
        Task<Result<Customer>> AddAsync(Customer customer);
        Task<Result<Customer>> UpdateAsync(Customer customer);
        Task<Result> DeleteAsync(int id);
    }
}