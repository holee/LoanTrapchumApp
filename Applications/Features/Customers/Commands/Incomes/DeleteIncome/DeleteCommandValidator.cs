using FluentValidation;

namespace Applications.Features.Customers.Commands.Incomes.DeleteIncome;

public sealed class DeleteCommandValidator : AbstractValidator<DeleteIncomeCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(x=>x.Id).GreaterThan(0).WithMessage("Id cannot be empty");
        RuleFor(x=>x.CustomerId).NotNull().WithMessage("CustomerId cannot be empty");
    }
}