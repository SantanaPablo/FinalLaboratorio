using FluentValidation;
using MSCustomer.Application.DTOs;
using MSCustomer.Domain;
using MSCustomer.Infrastructure.Repositories;

namespace MSCustomer.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;

        public CustomerService(
            ICustomerRepository repository,
            IValidator<CreateCustomerDto> createValidator,
            IValidator<UpdateCustomerDto> updateValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createDto)
        {
            // Validar usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Fail<CustomerDto>(errors);
            }

            var customer = new Customer
            {
                Name = createDto.Name,
                Email = createDto.Email,
                Address = createDto.Address ?? string.Empty
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
            // Validar usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Fail<CustomerDto>(errors);
            }

            var existingResult = await _repository.GetByIdAsync(id);
            if (!existingResult.IsSuccess)
                return Result.Fail<CustomerDto>(existingResult.Error);

            var customer = existingResult.Value;
            customer.Name = updateDto.Name;
            customer.Email = updateDto.Email;
            customer.Address = updateDto.Address ?? string.Empty;

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

        public async Task<Result> DeleteCustomerAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}