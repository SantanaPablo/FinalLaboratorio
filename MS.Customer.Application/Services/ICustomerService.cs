using MSCustomer.Application.DTOs;
using MSCustomer.Domain;

namespace MSCustomer.Application.Services
{
    public interface ICustomerService
    {
        Task<Result<IEnumerable<CustomerDto>>> GetAllCustomersAsync();
        Task<Result<CustomerDto>> GetCustomerByIdAsync(int id);
        Task<Result<CustomerDto>> GetCustomerByEmailAsync(string email);
        Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createDto);
        Task<Result<CustomerDto>> UpdateCustomerAsync(int id, UpdateCustomerDto updateDto);
        Task<Result> DeleteCustomerAsync(int id);
    }
}