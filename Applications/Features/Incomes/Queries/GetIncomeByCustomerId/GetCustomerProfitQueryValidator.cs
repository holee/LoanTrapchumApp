using Applications.Features.Expenses.Queries.GetExpenseByCustomerId;
using FluentValidation;

namespace Applications.Features.Incomes.Queries.GetIncomeByCustomerId
{
    public sealed class GetCustomerProfitQueryValidator : AbstractValidator<GetExpenseByCustomerQuery>
    {
        public GetCustomerProfitQueryValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty()
                                    .WithMessage("Please provide customerId");
        }
    }
}
