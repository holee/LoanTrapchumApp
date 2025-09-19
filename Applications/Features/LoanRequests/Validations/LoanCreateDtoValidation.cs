using Applications.Features.Loans.Requests;
using FluentValidation;
namespace Applications.Validations.Loans
{
    public class LoanCreateDtoValidator : AbstractValidator<LoanDtoCreate>
    {
        public LoanCreateDtoValidator()
        {
            RuleFor(c => c.CustomerRefId).NotEmpty().WithMessage("Please select customer")
                                       .NotNull().WithMessage("Please selet customer");
            RuleFor(c => c.InterestRate).GreaterThan(0).WithMessage("Please input interest rate");
        }
    }


}
