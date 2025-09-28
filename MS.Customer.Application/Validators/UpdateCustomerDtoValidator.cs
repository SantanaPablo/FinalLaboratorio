using FluentValidation;
using MSCustomer.Application.DTOs;

namespace MS.Customer.Application.Validators
{
    public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido")
                .EmailAddress().WithMessage("El formato del email no es válido")
                .MaximumLength(250).WithMessage("El email no puede exceder 250 caracteres");

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("La dirección no puede exceder 500 caracteres");
        }
    }
}