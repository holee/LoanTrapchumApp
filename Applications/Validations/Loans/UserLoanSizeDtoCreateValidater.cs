using Applications.Requests.SysSettings;
using FluentValidation;
namespace Applications.Validations.Loans
{
    public class UserLoanSizeDtoCreateValidater : AbstractValidator<UserLoanSizeDtoCreate>
    {
        public UserLoanSizeDtoCreateValidater()
        {
            RuleFor(c => c.UserRefId).NotEmpty().WithMessage("Please select User")
                                     .GreaterThan(0).WithMessage("Please selet User");

            RuleFor(c => c.MinLoanSize).NotEmpty().WithMessage("Please privide Min Loan Sise.")
                                      .GreaterThan(0).WithMessage("Max Loan size must be greater than 0.");
 
            RuleFor(c => c.StartDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Start Date must be greate than current date.");

            RuleFor(c => c.MaxLoanSize)
                                    .NotEmpty().WithMessage("Please privide Max Loan Sise.")
                                    .GreaterThan(0).WithMessage("Max Loan size must be greater than 0.");
        }
    }



}
