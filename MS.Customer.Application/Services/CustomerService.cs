using MSCustomer.Application.DTOs;
using MSCustomer.Domain;
using MSCustomer.Infrastructure.Repositories;
using MSCustomer.Application.Services;

namespace MSCustomer.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<CustomerDto>>> GetAllCustomersAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess)
                return Result.Fail<IEnumerable<CustomerDto>>(result.Error);

            var customerDtos = result.Value.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Address = c.Address,
                RegistrationDate = c.RegistrationDate
            });

            return Result.Success<IEnumerable<CustomerDto>>(customerDtos);
        }

        public async Task<Result<CustomerDto>> GetCustomerByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (!result.IsSuccess)
                return Result.Fail<CustomerDto>(result.Error);

            var customerDto = new CustomerDto
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Email = result.Value.Email,
                Address = result.Value.Address,
                RegistrationDate = result.Value.RegistrationDate
            };

            return Result.Success(customerDto);
        }

        public async Task<Result<CustomerDto>> GetCustomerByEmailAsync(string email)
        {
            var result = await _repository.GetByEmailAsync(email);
            if (!result.IsSuccess)
                return Result.Fail<CustomerDto>(result.Error);

            var customerDto = new CustomerDto
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Email = result.Value.Email,
                Address = result.Value.Address,
                RegistrationDate = result.Value.RegistrationDate
            };

            return Result.Success(customerDto);
        }

        public async Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createDto)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(createDto.Name))
                return Result.Fail<CustomerDto>("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(createDto.Email))
                return Result.Fail<CustomerDto>("El email es requerido");

            if (!IsValidEmail(createDto.Email))
                return Result.Fail<CustomerDto>("El formato del email no es válido");

            var customer = new Customer
            {
                Name = createDto.Name.Trim(),
                Email = createDto.Email.Trim().ToLower(),
                Address = createDto.Address?.Trim() ?? string.Empty
            };

            var result = await _repository.AddAsync(customer);
            if (!result.IsSuccess)
                return Result.Fail<CustomerDto>(result.Error);

            var customerDto = new CustomerDto
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Email = result.Value.Email,
                Address = result.Value.Address,
                RegistrationDate = result.Value.RegistrationDate
            };

            return Result.Success(customerDto);
        }

        public async Task<Result<CustomerDto>> UpdateCustomerAsync(int id, UpdateCustomerDto updateDto)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(updateDto.Name))
                return Result.Fail<CustomerDto>("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(updateDto.Email))
                return Result.Fail<CustomerDto>("El email es requerido");

            if (!IsValidEmail(updateDto.Email))
                return Result.Fail<CustomerDto>("El formato del email no es válido");

            var existingResult = await _repository.GetByIdAsync(id);
            if (!existingResult.IsSuccess)
                return Result.Fail<CustomerDto>(existingResult.Error);

            var customer = existingResult.Value;
            customer.Name = updateDto.Name.Trim();
            customer.Email = updateDto.Email.Trim().ToLower();
            customer.Address = updateDto.Address?.Trim() ?? string.Empty;

            var result = await _repository.UpdateAsync(customer);
            if (!result.IsSuccess)
                return Result.Fail<CustomerDto>(result.Error);

            var customerDto = new CustomerDto
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Email = result.Value.Email,
                Address = result.Value.Address,
                RegistrationDate = result.Value.RegistrationDate
            };

            return Result.Success(customerDto);
        }

        public async Task<Result> DeleteCustomerAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}