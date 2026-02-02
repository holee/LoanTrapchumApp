using Applications.Features.Departments.Commands.CreateDepartment;

namespace Applications.Features.Departments.Commands.EditDepartment;

public sealed class EditDepartmentCommandValidator : AbstractValidator<EditDeparmentCommand>
{
    public EditDepartmentCommandValidator()
    {
        RuleFor(x => x.Request.DepartmentName)
            .NotEmpty().WithMessage("Department Name is required.")
            .MaximumLength(100).WithMessage("Department Name must not exceed 100 characters.");
        //RuleFor(x => x.Request.BranchId)
        //    .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
    }
}