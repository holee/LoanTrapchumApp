using FluentValidation;

namespace Applications.Requests.Validations.Loans;

public class UserLoanSizeDtoCreateValidater : AbstractValidator<UserLoanSizeDtoCreate>
{
    public UserLoanSizeDtoCreateValidater()
    {
        RuleFor(c => c.UserRefId).NotEmpty().WithMessage("Please select User");

        RuleFor(c => c.MinUsdLoanSize).Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Please privide Min Loan Sise.")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Min Loan size must be greater than or equal to 0.");

        RuleFor(c => c.MaxUsdLoanSize).Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                    .WithMessage("Please privide Max Loan Sise.")
                                .GreaterThan(0)
                                    .WithMessage("Max Loan size must be greater than 0.")
                                .Must((p, maxValue) => MaxUsdValue(p, maxValue))
                                    .WithMessage("Min Usd value must smaller than Max Usd value.");

        RuleFor(c => c.MinKhrLoanSize).Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                    .WithMessage("Please privide Min Khr Loan Sise.")
                                .GreaterThanOrEqualTo(0)
                                    .WithMessage("Min Khr Loan size must be greater than or equal to 0.");

        RuleFor(c => c.MaxKhrLoanSize).Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                    .WithMessage("Please privide Max Loan Sise.")
                                .GreaterThan(0)
                                    .WithMessage("Max Loan size must be greater than 0.")
                                .Must((p, maxValue) => MaxKhrValue(p, maxValue))
                                    .WithMessage("Min Khr Value must smaller than Max Khr");

        RuleFor(c => c.StartDate).Cascade(CascadeMode.Stop)
                     .GreaterThanOrEqualTo(DateTime.Now)
                     .WithMessage("Start Date must be greate than current date.");


        RuleFor(c => c.EndDate).Cascade(CascadeMode.Stop)
                    .GreaterThan(DateTime.Now)
                        .WithMessage("Start Date must be greate than current date.")
                    .Must((p, future) => MustFutureDate(p, future))
                        .WithMessage("{property} must greater than {property}");
    }



    private bool MaxKhrValue(UserLoanSizeDtoCreate p, decimal? maxValue)
    {
        return maxValue > p.MinKhrLoanSize;
    }

    private bool MaxUsdValue(UserLoanSizeDtoCreate p, decimal? mxValue)
    {
        return mxValue > p.MinUsdLoanSize;
    }

    private bool MustFutureDate(UserLoanSizeDtoCreate p, DateTime? future)
    {
        var now = DateTime.Now;

        var baseDate = p.StartDate ?? now;

        return (future ?? now) > baseDate;
    }


}
