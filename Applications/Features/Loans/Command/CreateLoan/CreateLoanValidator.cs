using FluentValidation;

namespace Applications.Features.Loans.Command.CreateLoan;

public class CreateLoanValidator : AbstractValidator<LoanDtoCreate>
{
    public CreateLoanValidator()
    {
        RuleFor(c => c.CustomerRefId).NotEmpty()
                                        .WithMessage("Please select customer")
                                     .NotNull()
                                        .WithMessage("Please selet customer");
        RuleFor(c => c.LoanRequestId).NotEmpty()
                                        .WithMessage("Please request amount.")
                                     .NotNull()
                                        .WithMessage("Please request amount.");
        RuleFor(c => c.AdminFeeRate).GreaterThan(0)
                                      .WithMessage("Please input admin rate");
        RuleFor(c => c.AdminFeeAmount).GreaterThan(0)
                                     .WithMessage("Please input admin rate");
        RuleFor(c => c.Installments).NotEmpty()
                                        .WithMessage("Installment is required")
                                    .GreaterThan(0)
                                        .WithMessage("Installment is required");
        RuleFor(c => c.InterestRate).GreaterThan(0)
                                .WithMessage("Please input interest rate");
        RuleFor(c => c.AprovedUserId).NotEmpty().WithMessage("Please select approval user")
                                     .WithMessage("Please select approval user");
        RuleFor(c => c.Circle).GreaterThan(0).WithMessage("Circle is required.");


        RuleFor(c => c.StartDate).Cascade(CascadeMode.Stop)
                                    .NotNull()
                                        .WithMessage("Please select start date.")
                                    .GreaterThanOrEqualTo(DateTime.Today)
                                        .WithMessage("Start date must be today or a future date.");

        RuleFor(c => c.FirstPaymentDate).Cascade(CascadeMode.Stop)
                                    .NotNull()
                                        .WithMessage("Please select first payment date.")
                                    .Must((model, firstPaymentDate) => firstPaymentDate >= model.StartDate)
                                        .WithMessage("First payment date must be greater than or equal to start date.");
    }
}
