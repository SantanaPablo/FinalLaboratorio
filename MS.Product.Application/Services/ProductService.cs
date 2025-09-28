using FluentValidation;
using MSProduct.Application.DTOs;
using MSProduct.Application.Validators;
using MSProduct.Domain;
using MSProduct.Infrastructure.Repositories;

namespace MSProduct.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;

        public ProductService(
            IProductRepository repository,
            IValidator<CreateProductDto> createValidator,
            IValidator<UpdateProductDto> updateValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result<ProductDto>> CreateProductAsync(CreateProductDto createDto)
        {
            // Validar usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Fail<ProductDto>(errors);
            }

            var product = new Domain.Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                StockQuantity = createDto.StockQuantity
            };

            var result = await _repository.AddAsync(product);
            if (!result.IsSuccess)
                return Result.Fail<ProductDto>(result.Error);

            var productDto = new ProductDto
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Description = result.Value.Description,
                Price = result.Value.Price,
                StockQuantity = result.Value.StockQuantity
            };

            return Result.Success(productDto);
        }

        public async Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto updateDto)
        {
            // Validar usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Fail<ProductDto>(errors);
            }

            var existingResult = await _repository.GetByIdAsync(id);
            if (!existingResult.IsSuccess)
                return Result.Fail<ProductDto>(existingResult.Error);

            var product = existingResult.Value;
            product.Name = updateDto.Name;
            product.Description = updateDto.Description;
            product.Price = updateDto.Price;
            product.StockQuantity = updateDto.StockQuantity;

            var result = await _repository.UpdateAsync(product);
            if (!result.IsSuccess)
                return Result.Fail<ProductDto>(result.Error);

            var productDto = new ProductDto
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Description = result.Value.Description,
                Price = result.Value.Price,
                StockQuantity = result.Value.StockQuantity
            };

            return Result.Success(productDto);
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess)
                return Result.Fail<IEnumerable<ProductDto>>(result.Error);

            var productDtos = result.Value.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            });

            return Result.Success<IEnumerable<ProductDto>>(productDtos);
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (!result.IsSuccess)
                return Result.Fail<ProductDto>(result.Error);

            var productDto = new ProductDto
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Description = result.Value.Description,
                Price = result.Value.Price,
                StockQuantity = result.Value.StockQuantity
            };

            return Result.Success(productDto);
        }

        public async Task<Result> DeleteProductAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<Result> UpdateStockAsync(int productId, int quantity)
        {
            if (quantity <= 0)
                return Result.Fail("La cantidad debe ser mayor a 0");

            return await _repository.UpdateStockAsync(productId, quantity);
        }
    }
}