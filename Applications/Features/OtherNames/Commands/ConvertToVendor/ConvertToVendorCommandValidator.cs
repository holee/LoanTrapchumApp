namespace Applications.Features.OtherNames.Commands.ConvertToVendor;

public sealed class ConvertToVendorCommandValidator : AbstractValidator<ConvertToVendorCommand>
{
    public ConvertToVendorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
