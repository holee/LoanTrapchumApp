using FluentValidation;

namespace Application.Features.Banks.Commands.DeleteBank;

/// <summary>
/// Validator for DeleteBankCommand
/// </summary>
public sealed class DeleteBankCommandValidator : AbstractValidator<DeleteBankCommand>
{
    public DeleteBankCommandValidator()
    {
        RuleFor(x => x.BankId)
            .GreaterThan(0)
            .WithMessage("Bank ID must be greater than 0");
    }
}
