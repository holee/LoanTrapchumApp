using Applications.Features.OtherNames.Dto;

namespace Applications.Features.OtherNames.Commands.CreateOtherName;

public sealed class CreateOtherNameCommandValidator : AbstractValidator<CreateOtherNameCommand>
{
    public CreateOtherNameCommandValidator()
    {
        RuleFor(x => x.Dto.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.Dto.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Dto.Phone)
            .MaximumLength(20).WithMessage("Phone must not exceed 20 characters.");

        RuleFor(x => x.Dto.EmailAddress)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Dto.EmailAddress))
            .WithMessage("Invalid email address format.");
    }
}
