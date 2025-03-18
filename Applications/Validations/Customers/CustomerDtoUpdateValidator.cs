using Applications.Requests.Lists.Customers;
using FluentValidation;
namespace Applications.Validations.Customers
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
