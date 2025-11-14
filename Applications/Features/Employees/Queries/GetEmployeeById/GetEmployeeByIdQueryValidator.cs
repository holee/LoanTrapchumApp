using FluentValidation;

namespace Applications.Features.Employees.Queries.GetEmployeeById;

public sealed class GetEmployeeByIdQueryValidator : AbstractValidator<GetEmployeeByIdQuery>
{
    public GetEmployeeByIdQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("EmployeeId is required.");
    }
}
