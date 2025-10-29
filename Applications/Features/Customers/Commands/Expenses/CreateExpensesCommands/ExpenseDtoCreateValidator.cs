using Applications.Features.Customers.Commands.Expenses.Dto;
using FluentValidation;

namespace Applications.Features.Customers.Commands.Expenses.CreateExpensesCommands;

public sealed class ExpenseDtoCreateValidator : AbstractValidator<ExpenseDtoCreate>
{
    public ExpenseDtoCreateValidator()
    {
        RuleFor(x => x.Amount)
           .GreaterThan(0)
           .WithMessage("Expense amount must be greater than zero.");

        RuleFor(x => x.ExpenseTitle)
            .NotEmpty()
            .WithMessage("Expense title is required.");
    }
}