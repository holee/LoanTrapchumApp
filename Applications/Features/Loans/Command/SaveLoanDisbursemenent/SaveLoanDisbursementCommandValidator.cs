namespace Applications.Features.Loans.Command.SaveLoanDisbursemenent;

public class SaveLoanDisbursementCommandValidator : AbstractValidator<SaveLoanDisbursementCommand>
{
    public SaveLoanDisbursementCommandValidator()
    {
        RuleFor(x => x.Request).NotNull().WithMessage("Request cannot be null.");
        RuleFor(x => x.Request).SetValidator(new SaveLoanDisbursementDtoCreateValidator());
    }
}
