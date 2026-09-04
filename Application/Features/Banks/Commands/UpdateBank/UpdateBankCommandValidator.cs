using Application.Features.Banks.Dtos;
using FluentValidation;

namespace Application.Features.Banks.Commands.UpdateBank;

/// <summary>
/// Validator for UpdateBankCommand
/// </summary>
public sealed class UpdateBankCommandValidator : AbstractValidator<UpdateBankCommand>
{
    public UpdateBankCommandValidator()
    {
        RuleFor(x => x.BankId)
            .GreaterThan(0)
            .WithMessage("Bank ID must be greater than 0");

        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Bank request cannot be null")
            .SetValidator(new BankDtoUpdateValidator());
    }
}

/// <summary>
/// Validator for BankDtoUpdate
/// </summary>
public sealed class BankDtoUpdateValidator : AbstractValidator<BankDtoUpdate>
{
    public BankDtoUpdateValidator()
    {
        RuleFor(x => x.BankId)
            .GreaterThan(0)
            .WithMessage("Bank ID must be greater than 0");

        RuleFor(x => x.BankCode)
            .NotEmpty()
            .WithMessage("Bank code is required")
            .MaximumLength(20)
            .WithMessage("Bank code must not exceed 20 characters");

        RuleFor(x => x.BankName)
            .NotEmpty()
            .WithMessage("Bank name is required")
            .MaximumLength(100)
            .WithMessage("Bank name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email address format");

        RuleFor(x => x.WebSite)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.WebSite))
            .WithMessage("Website must not exceed 200 characters");

        RuleFor(x => x.AltPhone)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.AltPhone))
            .WithMessage("Phone number must not exceed 20 characters");
    }
}
