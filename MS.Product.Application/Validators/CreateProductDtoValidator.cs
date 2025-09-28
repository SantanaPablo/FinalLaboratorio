using FluentValidation;
using MSProduct.Application.DTOs;

namespace MSProduct.Application.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0")
                .LessThanOrEqualTo(999999999).WithMessage("El precio no puede exceder $999,999,999");
            //el precio máximo va a depender del tipo de moneda

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo")
                .LessThanOrEqualTo(99999).WithMessage("El stock no puede exceder 99,999 unidades");
        }
    }
}