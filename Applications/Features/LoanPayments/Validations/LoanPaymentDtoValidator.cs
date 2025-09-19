using Applications.Features.LoanPayments.Requests;
using FluentValidation;

namespace Applications.Features.LoanPayments.Validations
{
    public sealed class LoanPaymentDtoValidator : AbstractValidator<LoanPaymentDtoCreate>
    {
        public LoanPaymentDtoValidator()
        {
            RuleFor(x => x.TotalAmount).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("Total Amount is required.")
                    .GreaterThan(0).WithMessage("Total Amount must be greater than zero.");

            RuleFor(x => x.RecieveAmount).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("Recieve Amount is required.")
                    .GreaterThan(0).WithMessage("Recieve Amount must be greater than zero.");

            // ReceiveAmount > TotalAmount
            RuleFor(x => x)
                .Must(x => x.RecieveAmount.HasValue && x.TotalAmount.HasValue &&
                           x.RecieveAmount.Value > x.TotalAmount.Value)
                .WithMessage("ReceiveAmount must be greater than TotalAmount.")
                .When(x => x.RecieveAmount.HasValue && x.TotalAmount.HasValue);
            
            // Rule 2: If PenaltyAmount > 0, it must not be greater than OriginalPenaltyAmount
            RuleFor(x => x.PenaltyAmount)
                .Must((model, penalty) =>
                {
                    if (penalty is > 0 && model.PaidPenaltyAmount.HasValue)
                    {
                        return penalty.Value <= model.PaidPenaltyAmount.Value;
                    }
                    return true; // skip validation if conditions not met
                })
                .WithMessage("PenaltyAmount must not be greater than OriginalPenaltyAmount.");
        }

    }
}
