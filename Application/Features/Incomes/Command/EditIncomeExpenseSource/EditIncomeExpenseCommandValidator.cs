using Application.Features.Incomes.Command.EditIncomeExpenseSource;
using FluentValidation;
namespace Application.Features.Incomes.Command.CreateIncomeExpenseSource;

public sealed class EditIncomeExpenseCommandValidator : AbstractValidator<EditIncomeExpenseCommand>
{
    public EditIncomeExpenseCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new EditIncomeExpeseValidator()); 
    }
}
