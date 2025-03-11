using Applications.Requests.Lists.Customers;
using FluentValidation;

namespace Applications.Requests.Validations.Customers
{

    public class CustomerDtoCreateValidator : AbstractValidator<CustomerDtoCreate>
    {
        public CustomerDtoCreateValidator()
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
