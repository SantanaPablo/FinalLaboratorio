using Microsoft.EntityFrameworkCore;
using MSCustomer.Domain;
using MSCustomer.Infrastructure.Data;
using MSCustomer.Infrastructure.Repositories;

namespace MSCustomer.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Customer>>> GetAllAsync()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                return Result.Success<IEnumerable<Customer>>(customers);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<Customer>>(ex.Message);
            }
        }

        public async Task<Result<Customer>> GetByIdAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                return customer != null
                    ? Result.Success(customer)
                    : Result.Fail<Customer>("Cliente no encontrado");
            }
            catch (Exception ex)
            {
                return Result.Fail<Customer>(ex.Message);
            }
        }

        public async Task<Result<Customer>> GetByEmailAsync(string email)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
                return customer != null
                    ? Result.Success(customer)
                    : Result.Fail<Customer>("Cliente no encontrado con ese email");
            }
            catch (Exception ex)
            {
                return Result.Fail<Customer>(ex.Message);
            }
        }

        public async Task<Result<Customer>> AddAsync(Customer customer)
        {
            try
            {
                // Verificar email único
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email.ToLower() == customer.Email.ToLower());

                if (existingCustomer != null)
                    return Result.Fail<Customer>("Ya existe un cliente con ese email");

                customer.RegistrationDate = DateTime.UtcNow;
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return Result.Success(customer);
            }
            catch (Exception ex)
            {
                return Result.Fail<Customer>(ex.Message);
            }
        }

        public async Task<Result<Customer>> UpdateAsync(Customer customer)
        {
            try
            {
                var existingCustomer = await _context.Customers.FindAsync(customer.Id);
                if (existingCustomer == null)
                    return Result.Fail<Customer>("Cliente no encontrado");

                // Verificar email único (excluyendo el cliente actual)
                var customerWithSameEmail = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email.ToLower() == customer.Email.ToLower() && c.Id != customer.Id);

                if (customerWithSameEmail != null)
                    return Result.Fail<Customer>("Ya existe otro cliente con ese email");

                existingCustomer.Name = customer.Name;
                existingCustomer.Email = customer.Email;
                existingCustomer.Address = customer.Address;
                existingCustomer.ModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Result.Success(existingCustomer);
            }
            catch (Exception ex)
            {
                return Result.Fail<Customer>(ex.Message);
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                    return Result.Fail("Cliente no encontrado");

                _context.Customers.Remove(customer);
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