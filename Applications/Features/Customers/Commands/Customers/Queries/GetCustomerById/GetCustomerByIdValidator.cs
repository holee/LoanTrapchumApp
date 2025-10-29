using Applications.Commons.Messages.Customers;
using FluentValidation;

namespace Applications.Features.Customers.Commands.Customers.Queries.GetCustomerById
{
    public sealed class GetCustomerByIdValidator : AbstractValidator<GetCustomerByIdQuery>
    {
        public GetCustomerByIdValidator()
        {
            RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(CustomerMessage.Required)
                .NotEmpty().WithMessage(CustomerMessage.Required);
        }
    }
}
