using MSOrder.Application.DTOs;
using MSOrder.Domain;
using MSOrder.Infrastructure.Repositories;
using MSOrder.Application.Services;


namespace MSOrder.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public OrderService(
            IOrderRepository repository,
            ICustomerService customerService,
            IProductService productService)
        {
            _repository = repository;
            _customerService = customerService;
            _productService = productService;
        }

        public async Task<Result<IEnumerable<OrderDto>>> GetAllOrdersAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess)
                return Result.Fail<IEnumerable<OrderDto>>(result.Error);

            var orderDtos = result.Value.Select(MapToOrderDto);
            return Result.Success<IEnumerable<OrderDto>>(orderDtos);
        }

        public async Task<Result<OrderDto>> GetOrderByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (!result.IsSuccess)
                return Result.Fail<OrderDto>(result.Error);

            var orderDto = MapToOrderDto(result.Value);
            return Result.Success(orderDto);
        }

        public async Task<Result<IEnumerable<OrderDto>>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var result = await _repository.GetByCustomerIdAsync(customerId);
            if (!result.IsSuccess)
                return Result.Fail<IEnumerable<OrderDto>>(result.Error);

            var orderDtos = result.Value.Select(MapToOrderDto);
            return Result.Success<IEnumerable<OrderDto>>(orderDtos);
        }

        public async Task<Result<OrderDto>> CreateOrderAsync(CreateOrderDto createDto)
        {
            // Validaciones básicas
            if (createDto.CustomerId <= 0)
                return Result.Fail<OrderDto>("ID de cliente inválido");

            if (!createDto.OrderItems.Any())
                return Result.Fail<OrderDto>("La orden debe tener al menos un producto");

            // Verificar que el cliente existe
            var customer = await _customerService.GetCustomerByIdAsync(createDto.CustomerId);
            if (customer == null)
                return Result.Fail<OrderDto>("Cliente no encontrado");

            var order = new Domain.Order
            {
                CustomerId = createDto.CustomerId,
                CustomerName = customer.Name,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            // Procesar cada item de la orden
            foreach (var item in createDto.OrderItems)
            {
                if (item.Quantity <= 0)
                    return Result.Fail<OrderDto>($"La cantidad para el producto {item.ProductId} debe ser mayor a 0");

                // Verificar que el producto existe
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product == null)
                    return Result.Fail<OrderDto>($"Producto con ID {item.ProductId} no encontrado");

                // Verificar stock disponible
                if (!await _productService.CheckStockAvailabilityAsync(item.ProductId, item.Quantity))
                {
                    return Result.Fail<OrderDto>($"Stock insuficiente para el producto {product.Name}. Stock disponible: {product.StockQuantity}");
                }

                // Determinar la cantidad a comprar (no puede ser mayor al stock disponible)
                var quantityToBuy = Math.Min(item.Quantity, product.StockQuantity);
                var subtotal = product.Price * quantityToBuy;

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantityToBuy,
                    Subtotal = subtotal
                };

                order.OrderItems.Add(orderItem);
                totalAmount += subtotal;
            }

            order.TotalAmount = totalAmount;

            // Guardar la orden
            var createResult = await _repository.AddAsync(order);
            if (!createResult.IsSuccess)
                return Result.Fail<OrderDto>(createResult.Error);

            // Actualizar stock de los productos
            foreach (var item in order.OrderItems)
            {
                await _productService.UpdateStockAsync(item.ProductId, item.Quantity);
            }

            var orderDto = MapToOrderDto(createResult.Value);
            return Result.Success(orderDto);
        }

        public async Task<Result> DeleteOrderAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static OrderDto MapToOrderDto(Domain.Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Subtotal = item.Subtotal
                }).ToList()
            };
        }
    }
}