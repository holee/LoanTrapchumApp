using Applications.Requests.Journals;
using Applications.Requests.Settings;
using FluentValidation;

namespace Applications.Requests.Validations.Loans
{
    public class UserLoanSizeDtoCreateValidater : AbstractValidator<UserLoanSizeDtoCreate>
    {
        public UserLoanSizeDtoCreateValidater()
        {
            RuleFor(c => c.UserRefId).NotEmpty().WithMessage("Please select User");
            RuleFor(c => c.MinUsdLoanSize).NotEmpty().WithMessage("Please privide Min Loan Sise.")
                                      .GreaterThan(0).WithMessage("Max Loan size must be greater than 0.")
                                      .Must((p,maxvalue)=>MinUsdValue(p,maxvalue))
                                      .WithMessage("Min Usd value must smaller than Max Usd value.");
            RuleFor(c => c.MaxUsdLoanSize)
                                    .NotEmpty().WithMessage("Please privide Max Loan Sise.")
                                    .GreaterThan(0).WithMessage("Max Loan size must be greater than 0.");
            RuleFor(c => c.MinKhrLoanSize)
                                    .NotEmpty().WithMessage("Please privide Min Khr Loan Sise.")
                                    .GreaterThan(0).WithMessage("Min Khr Loan size must be greater than 0.")
                                    .Must((p,maxValue)=>MinKhrValue(p,maxValue))
                                    .WithMessage("Min Khr Value must smaller than Max Khr");
            RuleFor(c => c.MaxKhrLoanSize)
                                    .NotEmpty().WithMessage("Please privide Max Loan Sise.")
                                    .GreaterThan(0).WithMessage("Max Loan size must be greater than 0.");
            RuleFor(c => c.StartDate).LessThanOrEqualTo(DateTime.Now)
                         .WithMessage("Start Date must be greate than current date.");
        }



        private bool MinKhrValue(UserLoanSizeDtoCreate p, decimal? maxValue) 
        {
            if (p.MinKhrLoanSize > 0 && p.MinKhrLoanSize < maxValue)
                return true;
            else
                return true;
        }

        private bool MinUsdValue(UserLoanSizeDtoCreate p, decimal? maxValue)
        {
            if (p.MinUsdLoanSize > 0 && p.MinUsdLoanSize < maxValue)
                return true;
            else
                return true;
        }



    }



}
