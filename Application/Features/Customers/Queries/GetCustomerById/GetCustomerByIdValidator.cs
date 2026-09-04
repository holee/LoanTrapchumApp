using Application.Commons.Messages.Customers;
using FluentValidation;

namespace Application.Features.Customers.Queries.GetCustomerById
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
