using FluentValidation;
using MSOrder.Application.DTOs;

namespace MSOrder.Application.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("El ID del cliente debe ser mayor a 0");

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("La orden debe tener al menos un producto")
                .Must(items => items.Count <= 100).WithMessage("La orden no puede tener más de 20 productos diferentes");
            //esto va a depender del limite que quieras ponerle a la orden

            RuleForEach(x => x.OrderItems).SetValidator(new CreateOrderItemDtoValidator());
        }
    }

    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("El ID del producto debe ser mayor a 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0")
                .LessThanOrEqualTo(1000).WithMessage("La cantidad no puede exceder 1000 unidades");
            // depende el limite maximo de unidades a decidir
        }
    }
}