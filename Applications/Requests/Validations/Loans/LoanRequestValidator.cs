using Applications.Requests.Loans.LoanRequests;
using FluentValidation;

namespace Applications.Requests.Validations.Loans;

public class LoanRequestValidator : AbstractValidator<LoanRequestDtoCreate>
{
    public LoanRequestValidator()
    {
        RuleFor(x => x.CustomerRefId).NotEmpty().WithMessage("Please select customer.")
                                     .NotNull().WithMessage("Please select cusotmer");
        RuleFor(x => x.CurrencyRefId).NotEmpty().WithMessage("Please select currency.")
                                    .GreaterThan(0).WithMessage("Please select currency");
        RuleFor(x => x.AprovedUserId).NotEmpty().WithMessage("Please select Aproved User.")
                                     .WithMessage("Please select Aproved User");
        RuleFor(x => x.RequestAmount).NotEmpty().WithMessage("Principle must be has value.")
                                     .GreaterThan(0).WithMessage("Principle value must be greater than Zero");
        RuleFor(x => x.AdminFeeRate).NotEmpty().WithMessage("AdminFee rate must has value.")
                                    .GreaterThan(0).WithMessage("Admin Fee rate must be greater than Zero");
        RuleFor(x => x.InterestRate).NotEmpty().WithMessage("Insterate must has value.")
                                     .GreaterThan(0).WithMessage("Interate must be greater than Zero.");
        RuleFor(x => x.Installments).NotEmpty().WithMessage("Installment number can be blank.")
                                    .GreaterThan(0).WithMessage("Installment must be greater than Zero.");
    }
}
