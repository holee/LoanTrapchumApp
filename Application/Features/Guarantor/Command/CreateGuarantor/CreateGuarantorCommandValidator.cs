namespace Application.Features.Guarantor.Command.CreateGuarantor;

public sealed class CreateGuarantorCommandValidator : AbstractValidator<CreateGuarantorCommand>
{
    public CreateGuarantorCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new EntityGuarantorCreateValidator());
    }
}