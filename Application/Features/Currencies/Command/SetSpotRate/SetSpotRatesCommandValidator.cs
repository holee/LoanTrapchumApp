using Application.Features.Currencies.Command.SetSpotRates;

namespace Application.Features.Currencies.Command.SetSpotRate; 

public sealed class SetSpotRateCommandValidator : AbstractValidator<SetSpotRateCommand>
{
    public SetSpotRateCommandValidator()
    {
        RuleFor(x => x.SpotRates).NotNull().SetValidator(new SetSpotRateValidator()); 
    }

}
