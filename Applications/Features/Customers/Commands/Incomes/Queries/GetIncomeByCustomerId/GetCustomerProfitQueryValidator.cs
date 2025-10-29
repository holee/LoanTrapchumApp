using Applications.Features.Customers.Commands.ExpenseCommands.Queries.GetExpenseByCustomerId;
using FluentValidation;

namespace Applications.Features.Customers.Commands.Incomes.Queries.GetIncomeByCustomerId
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
