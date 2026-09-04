using Application.Features.Incomes.Command.GetEditIncomeExpenseSource;
using FluentValidation;
namespace Application.Features.Incomes.Queries.GetEditIncomeExpenseSource;

public sealed class GetEditIncomeExpenseCommandValidator : AbstractValidator<GetEditIncomeExpenseQuery>
{
    public GetEditIncomeExpenseCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("ID must be greater then zero.");
    }
}
