using FluentValidation;

namespace Application.Features.Banks.Queries.GetBankById;

/// <summary>
/// Validator for GetBankByIdQuery
/// </summary>
public sealed class GetBankByIdQueryValidator : AbstractValidator<GetBankByIdQuery>
{
    public GetBankByIdQueryValidator()
    {
        RuleFor(x => x.BankId)
            .GreaterThan(0)
            .WithMessage("Bank ID must be greater than 0");
    }
}
