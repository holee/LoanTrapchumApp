namespace Applications.Features.OtherNames.Commands.DeleteOtherName;

public sealed class DeleteOtherNameCommandValidator : AbstractValidator<DeleteOtherNameCommand>
{
    public DeleteOtherNameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
