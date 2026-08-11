using Applications.Features.LoanRequests.Requests;
using FluentValidation;

namespace Applications.Requests.Validations.Loans
{
    public class CreateLoanCommandValidation : AbstractValidator<LoanDtoCreate>
    {
        public CreateLoanCommandValidation()
        {
            RuleFor(c => c.CustomerRefId).NotEmpty().WithMessage("Please select customer")
                                       .NotNull().WithMessage("Please selet customer");
            RuleFor(c => c.InterestRate).GreaterThan(0).WithMessage("Please input interest rate");
        }
    }

    public sealed class LoanRequestCreateValidator : AbstractValidator<LoanRequestDtoCreate>
    {
        public LoanRequestCreateValidator()
        {
            RuleFor(c => c.CustomerRefId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                                            .WithMessage("Please select customer")
                                         .NotNull()
                                            .WithMessage("Please selet customer");
            RuleFor(c => c.RequestAmount).Cascade(CascadeMode.Stop).NotEmpty()
                                            .WithMessage("Please request amount.")
                                         .GreaterThan(0)
                                            .WithMessage("Please request amount.");
            RuleFor(c => c.AdminFeeRate).GreaterThan(0)
                                          .WithMessage("Please input admin rate");
            RuleFor(c => c.AdminFeeAmount).GreaterThan(0)
                                         .WithMessage("Please input admin rate");
            RuleFor(c => c.Installments).Cascade(CascadeMode.Stop).NotEmpty()
                                            .WithMessage("Installment is required")
                                        .GreaterThan(0)
                                            .WithMessage("Installment is required");
            RuleFor(c => c.InterestRate).GreaterThan(0)
                                    .WithMessage("Please input interest rate");
            RuleFor(c => c.AprovedUserId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Please select approval user")
                                         .WithMessage("Please select approval user");
            RuleFor(c => c.Circle).GreaterThan(0).WithMessage("Circle is required.");
        }
    }


}
