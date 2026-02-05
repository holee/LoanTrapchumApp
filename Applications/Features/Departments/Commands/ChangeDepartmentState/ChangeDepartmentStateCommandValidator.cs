namespace Applications.Features.Departments.Commands.ChangeDepartmentState;

public class ChangeDepartmentStateCommandValidator : AbstractValidator<ChangeDepartmentStateCommand>
{
    public ChangeDepartmentStateCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("DepartmentId must be greater than 0.");
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive must be provided.");
    }
}
