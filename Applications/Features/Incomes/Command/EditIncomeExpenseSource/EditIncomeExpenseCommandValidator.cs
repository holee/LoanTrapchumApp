using Applications.Features.Incomes.Command.EditIncomeExpenseSource;
using FluentValidation;
namespace Applications.Features.Incomes.Command.CreateIncomeExpenseSource;

public sealed class EditIncomeExpenseCommandValidator : AbstractValidator<EditIncomeExpenseCommand>
{
    public EditIncomeExpenseCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new EditIncomeExpeseValidator()); 
    }
}
