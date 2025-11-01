using FluentValidation;

namespace Applications.Features.Expenses.Queries.GetExpenseByCustomerId;

public sealed class GetExpenseByCusotmerQueryValidator : AbstractValidator<GetExpenseByCustomerQuery>
{

    public GetExpenseByCusotmerQueryValidator()
    {
        RuleFor(x => x.CustomerId)
                    .NotEmpty().WithMessage("Customer ID is required.");
    }

}