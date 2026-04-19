namespace Applications.Features.AdditionalInformations.Command.CreateSalutation;

public sealed class CreateSalutationCommandValidator : AbstractValidator<CreateSalutationCommand>
{
    public CreateSalutationCommandValidator()
    {
        RuleFor(command => command.Request)
            .NotNull()
            .WithMessage("Salutation create not created");
        RuleFor(command=>command.Request.Title)
            .NotNull()
            .WithMessage("Salutation create not created")
            .NotEmpty()
            .WithMessage("Salutation create not created");
    }
}