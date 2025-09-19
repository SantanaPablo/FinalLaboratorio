using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSProduct.Domain;
using MSProduct.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MSProduct.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Product>>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return Result.Success<IEnumerable<Product>>(products);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<Product>>(ex.Message);
            }
        }

        public async Task<Result<Product>> GetByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                return product != null
                    ? Result.Success(product)
                    : Result.Fail<Product>("Producto no encontrado");
            }
            catch (Exception ex)
            {
                return Result.Fail<Product>(ex.Message);
            }
        }

        public async Task<Result<Product>> AddAsync(Product product)
        {
            try
            {
                product.CreatedDate = DateTime.UtcNow;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return Result.Success(product);
            }
            catch (Exception ex)
            {
                return Result.Fail<Product>(ex.Message);
            }
        }

        public async Task<Result<Product>> UpdateAsync(Product product)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(product.Id);
                if (existingProduct == null)
                    return Result.Fail<Product>("Producto no encontrado");

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.StockQuantity = product.StockQuantity;
                existingProduct.ModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Result.Success(existingProduct);
            }
            catch (Exception ex)
            {
                return Result.Fail<Product>(ex.Message);
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return Result.Fail("Producto no encontrado");

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> UpdateStockAsync(int productId, int quantity)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                    return Result.Fail("Producto no encontrado");

                if (product.StockQuantity < quantity)
                    return Result.Fail("Stock insuficiente");

                product.StockQuantity -= quantity;
                product.ModifiedDate = DateTime.UtcNow;
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
