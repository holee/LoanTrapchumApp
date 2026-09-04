using FluentValidation;

namespace Application.Features.Expenses.DeleteExpenseCommand;

public sealed class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0)
            .WithMessage("Id must be greater than zero");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId cannot be empty");
    }
}