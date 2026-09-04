using FluentValidation;
namespace Application.Features.Incomes.Command.CreateIncomeExpenseSource;

public sealed class CreateIncomeExpenseCommandValidator : AbstractValidator<CreateIncomeExpenseCommand>
{
    public CreateIncomeExpenseCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new CreateIncomeExpeseValidator());
    }
}
