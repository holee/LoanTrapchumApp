using FluentValidation;

namespace Applications.Features.Customers.Commands.ExpenseCommands.Queries.GetExpenseByCustomerId;

public sealed class GetExpenseByCusotmerQueryValidator : AbstractValidator<GetExpenseByCustomerQuery>
{

    public GetExpenseByCusotmerQueryValidator()
    {
        RuleFor(x => x.CustomerId)
                    .NotEmpty().WithMessage("Customer ID is required.");
    }

}