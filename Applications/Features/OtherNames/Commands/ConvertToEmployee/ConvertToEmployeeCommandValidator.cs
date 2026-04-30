namespace Applications.Features.OtherNames.Commands.ConvertToEmployee;

public sealed class ConvertToEmployeeCommandValidator : AbstractValidator<ConvertToEmployeeCommand>
{
    public ConvertToEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
