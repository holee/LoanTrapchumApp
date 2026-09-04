using Application.Features.Currencies.Dto;

namespace Application.Features.Currencies.Command.EditCurrencyNote;

public sealed class CurrencyCounterDtoUpdateValidator : AbstractValidator<CurrencyNoteDtoUpdate>
{
    public CurrencyCounterDtoUpdateValidator()
    {

        RuleFor(x => x.Id).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id cannot be empty.");


        RuleFor(x => x.CurrencyRefId).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("CurrencyRefId is required.")
                    .GreaterThan(0).WithMessage("CurrencyRefId cannot be empty.");

        RuleFor(x => x.CurrencyValue).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("CurrencyValue is required.")
                    .GreaterThan(0).WithMessage("CurrencyValue must be greater than zero.");

        RuleFor(x => x.Quantity).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}