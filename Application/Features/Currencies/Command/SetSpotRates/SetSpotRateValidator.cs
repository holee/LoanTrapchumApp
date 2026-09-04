namespace Application.Features.Currencies.Command.SetSpotRates;

public sealed class SetSpotRateValidator : AbstractValidator<CurrencySpotRateDtoCreate>
{

    public SetSpotRateValidator()
    {
        RuleFor(x => x.BuyingRate).Cascade(CascadeMode.Stop)
                                .NotNull()
                                    .WithMessage("សូមបញ្ចូលអត្រាទិញ")
                                .GreaterThan(0)
                                     .WithMessage("អត្រាទិញត្រូវតែធំជាង 0");

        RuleFor(x => x.SellingRate).Cascade(CascadeMode.Stop)
                                .NotNull()
                                    .WithMessage("សូមបញ្ចូលអត្រាលក់")
                                .GreaterThan(0)
                                     .WithMessage("អត្រាលក់ត្រូវតែធំជាង 0");

        RuleFor(x => x.SpotRate).Cascade(CascadeMode.Stop)
                                .NotNull()
                                    .WithMessage("សូមបញ្ចូលអត្រាស្ពត")
                                .GreaterThan(0)
                                     .WithMessage("អត្រាស្ពតត្រូវតែធំជាង 0");

        RuleFor(x => x.TransactionDate).Cascade(CascadeMode.Stop)
                                .NotNull()
                                    .WithMessage("សូមបញ្ចូលកាលបរិច្ឆេទប្រតិបត្តិការ");
    }

}
