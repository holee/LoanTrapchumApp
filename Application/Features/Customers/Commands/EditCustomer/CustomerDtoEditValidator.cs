using Application.Features.Customers.Dto;
using FluentValidation;
namespace Application.Features.Customers.Commands.EditCustomer;
public sealed class CustomerDtoEditValidator : AbstractValidator<EntityCustomerCreate>
{
    public CustomerDtoEditValidator() 
    {
        RuleFor(x=>x.Id).NotEmpty().WithMessage("Customer Id cannot be null.");
        RuleFor(x => x.FirstName)
               .NotEmpty().WithMessage("Please input First Name Field")
               .Matches(@"^[\p{L}]+$").WithMessage("Name field alphabet only allow");
        RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Please input First Name Field")
                .Matches(@"^[\p{L}]+$").WithMessage("Name Field Alphabet only allow");
        RuleFor(x => x.AltName)
              .NotEmpty().WithMessage("Please Input Khmer Field");
        RuleFor(x => x.IdCard)
            .NotEmpty().WithMessage("Please Input ID Card Field");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Please Input Phone Field");
    }
}
