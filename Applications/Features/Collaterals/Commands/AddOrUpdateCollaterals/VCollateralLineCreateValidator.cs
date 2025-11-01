using Applications.Features.Collaterals.Dtos;
using FluentValidation;

namespace Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals;

public sealed class VCollateralLineCreateValidator : AbstractValidator<VCollateralLines>
{
    public VCollateralLineCreateValidator() 
    {
        RuleFor(x => x.OwnerName)
                   .NotEmpty().WithMessage("Owner Name Is Required.");

        RuleFor(x => x.CollateralNo)
                   .NotEmpty().WithMessage("Collateral Number Is Required.");

        RuleFor(x => x.PawnPrice).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("Pawn Price Is Required.")
                    .GreaterThan(0).WithMessage("Pawn Price must greater than Zero.");

        RuleFor(x => x.HotPrice)
                   .Cascade(CascadeMode.Stop)
                   .NotNull().WithMessage("Hot Price Is Required.")
                   .GreaterThan(0).WithMessage("Hot Price must greater than Zero.");

        RuleFor(x => x.MarketPrice)
                    .Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("Market Price Is Required.")
                   .GreaterThan(0).WithMessage("Market Price Must Greater than Zero.");


        RuleFor(x => x.CategoryRefId)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Category Is Required.")
                .GreaterThan(0).WithMessage("Category ID must greater than Zero.");


        RuleFor(x => x.SubCategoryRefId)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("SubCategory Is Required.")
                 .GreaterThan(0).WithMessage("Sub Category ID must greater than Zero.");

        // MarketPrice should be greater than PawnPrice and HotPrice
        RuleFor(x => x.MarketPrice)
            .Must((model, marketPrice) =>
                marketPrice > model.PawnPrice && marketPrice > model.HotPrice)
            .WithMessage("Market Price must be greater than Pawn Price and Hot Price.")
            .When(x => x.MarketPrice > 0 && x.PawnPrice > 0 && x.HotPrice > 0); ;

        // PawnPrice should not be greater than MarketPrice and HotPrice
        RuleFor(x => x.PawnPrice)
            .Must((model, pawnPrice) =>
                pawnPrice < model.MarketPrice && pawnPrice < model.HotPrice)
            .WithMessage("Pawn Price must not be greater than Market Price and Hot Price.")
            .When(x => x.PawnPrice > 0 && x.MarketPrice > 0 && x.HotPrice > 0); ;

        // HotPrice should not be greater than MarketPrice and PawnPrice
        RuleFor(x => x.HotPrice)
            .Must((model, hotPrice) =>
                hotPrice < model.MarketPrice && hotPrice > model.PawnPrice)
            .WithMessage("Hot Price must not be greater than Market Price and Pawn Price.")
             .When(x => x.HotPrice > 0 && x.MarketPrice > 0 && x.PawnPrice > 0);
    }
}
