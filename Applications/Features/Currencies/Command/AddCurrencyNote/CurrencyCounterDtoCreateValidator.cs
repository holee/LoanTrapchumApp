using Applications.Features.Currencies.Dto;

namespace Applications.Features.Currencies.Command.AddNote;

public sealed class CurrencyCounterDtoCreateValidator : AbstractValidator<CurrencyNoteDtoCreate>
{
    public CurrencyCounterDtoCreateValidator()
    {
        RuleFor(x => x.CurrencyRefId)
                    .NotNull().WithMessage("CurrencyRefId is required.")
                    .GreaterThan(0).WithMessage("CurrencyRefId cannot be empty.");

        RuleFor(x => x.CurrencyValue)
                    .NotNull().WithMessage("CurrencyValue is required.")
                    .GreaterThan(0).WithMessage("CurrencyValue must be greater than zero.");

        RuleFor(x => x.Quantity)
                .NotNull().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}