namespace Applications.Features.Employees.Commands.ChangeEmployeeState;

public sealed class ChangeEmployeeStateCommandValidator : AbstractValidator<ChangeEmployeeStateCommand>
{
    public ChangeEmployeeStateCommandValidator()
    {
        RuleFor(x => x.EmployeeId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("EmployeeId is required.")
            .MaximumLength(100).WithMessage("EmployeeId must not exceed 100 characters.");
    }
}
