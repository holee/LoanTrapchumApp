using Applications.Features.Customers.Commands.CustomerExpenseQuery;
using FluentValidation;

namespace Applications.Features.Customers.Commands.CustomerIncomeQuery
{
    public sealed class GetCustomerProfitQueryValidator : AbstractValidator<GetCustomerExpenseQuery>
    {
        public GetCustomerProfitQueryValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty()
                                    .WithMessage("Please provide customerId");
        }
    }
}
