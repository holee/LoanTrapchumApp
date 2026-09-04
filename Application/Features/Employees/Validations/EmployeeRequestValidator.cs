using Application.Features.Employees.Requests;
using FluentValidation;

namespace Application.Requests.Validations.Employees
{
    public class EmployeeDtoCreateValidator : AbstractValidator<EmployeeDtoCreate>
    {
        public EmployeeDtoCreateValidator()
        {
            RuleFor(x=>x.FirstName)
                    .NotEmpty().WithMessage("Please input First Name Field")
                    .Matches(@"^[\p{L}]+$").WithMessage("First Name field allows alphabet only");
            RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Please input Last Name Field")
                    .Matches(@"^[\p{L}]+$").WithMessage("Last Name field allows alphabet only");
        }
    }
}
