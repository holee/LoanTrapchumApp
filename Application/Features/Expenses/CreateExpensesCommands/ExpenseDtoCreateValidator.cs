using Application.Features.Expenses.Dto;
using FluentValidation;

namespace Application.Features.Expenses.CreateExpensesCommands;

public sealed class ExpenseDtoCreateValidator : AbstractValidator<ExpenseDtoCreate>
{
    public ExpenseDtoCreateValidator()
    {
        RuleFor(x => x.Amount)
            .NotNull().WithMessage("Expense amount is required.")
           .GreaterThan(0)
           .WithMessage("Expense amount must be greater than zero.");

        RuleFor(x => x.ExpenseTitle)
            .NotEmpty()
            .WithMessage("Expense title is required.");
    }
}