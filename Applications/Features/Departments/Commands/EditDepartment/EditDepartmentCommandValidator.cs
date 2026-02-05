using Applications.Features.Departments.Commands.CreateDepartment;

namespace Applications.Features.Departments.Commands.EditDepartment;

public sealed class EditDepartmentCommandValidator : AbstractValidator<EditDepartmentCommand>
{
    public EditDepartmentCommandValidator()
    {
        RuleFor(x => x.Request.DepartmentName)
            .NotEmpty().WithMessage("Department Name is required.")
            .MaximumLength(100).WithMessage("Department Name must not exceed 100 characters.");
    }
}