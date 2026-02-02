namespace Applications.Features.Departments.Commands.DeleteDepartment;

public sealed class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department ID is required.");
        //RuleFor(x => x.BranchId)
        //    .NotEmpty().WithMessage("Branch ID is required.");
    }
}