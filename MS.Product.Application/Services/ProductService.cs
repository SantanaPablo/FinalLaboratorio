using MSProduct.Application.DTOs;
using MSProduct.Domain;
using MSProduct.Infrastructure.Repositories;

namespace MSProduct.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
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

        public async Task<Result<ProductDto>> CreateProductAsync(CreateProductDto createDto)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(createDto.Name))
                return Result.Fail<ProductDto>("El nombre es requerido");

            if (createDto.Price <= 0)
                return Result.Fail<ProductDto>("El precio debe ser mayor a 0");

            if (createDto.StockQuantity < 0)
                return Result.Fail<ProductDto>("El stock no puede ser negativo");

            var product = new Product
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
            // Validaciones
            if (string.IsNullOrWhiteSpace(updateDto.Name))
                return Result.Fail<ProductDto>("El nombre es requerido");

            if (updateDto.Price <= 0)
                return Result.Fail<ProductDto>("El precio debe ser mayor a 0");

            if (updateDto.StockQuantity < 0)
                return Result.Fail<ProductDto>("El stock no puede ser negativo");

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