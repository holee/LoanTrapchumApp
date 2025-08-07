using Applications.Features.Customers.Requests;
using FluentValidation;
namespace Applications.Features.Customers.Validations
{
    public class CustomerDtoUpdateValidator : AbstractValidator<CustomerDtoUpdate> 
    {
        public CustomerDtoUpdateValidator() 
        {
            RuleFor(x => x.FirstName)
                   .NotEmpty().WithMessage("Please input First Name Field")
                   .Matches(@"^[\p{L}]+$").WithMessage("Name field alphabet anly allow");
            RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Please input First Name Field")
                    .Matches(@"^[\p{L}]+$").WithMessage("Name field alphabet anly allow");
        }
    }

}
