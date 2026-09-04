using Application.Commons.Messages.Customers;
using FluentValidation;

namespace Application.Features.Customers.Queries.GetCustomerById
{
    public sealed class GetUpdateCustomerByIdValidator : AbstractValidator<GetCustomerByIdQuery>
    {
        public GetUpdateCustomerByIdValidator()
        {
            RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(CustomerMessage.Required)
                .NotEmpty().WithMessage(CustomerMessage.Required);
        }
    }
}
