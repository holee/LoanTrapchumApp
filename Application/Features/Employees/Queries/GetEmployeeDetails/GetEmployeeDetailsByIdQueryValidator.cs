namespace Application.Features.Employees.Queries.GetEmployeeById;

public sealed class GetEmployeeDetailsByIdQueryValidator : AbstractValidator<GetEmployeeDetailsByIdQuery>
{
    public GetEmployeeDetailsByIdQueryValidator()
    {
        RuleFor(x => x.EmployeeId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Employee ID is required.")
            .NotNull().WithMessage("Employee ID cannot be null.");
    }
}