namespace Application.Features.Departments.Queries.GetDepartmentById;

public sealed class GetDepartmentByIdValidator : AbstractValidator<GetDepartmentById>
{
    public GetDepartmentByIdValidator()
    {
        RuleFor(x => x.DepartmentId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("DepartmentId is required.")
            .GreaterThan(0).WithMessage("DepartmentId must be greater than zero.");
        //RuleFor(x => x.BranchId)
        //    .NotEmpty().WithMessage("BranchId is required.")
        //    .GreaterThan(0).WithMessage("BranchId must be greater than zero.");
    }
}
