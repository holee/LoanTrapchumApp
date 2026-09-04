using Application.Features.LoanRequests.Requests;
using FluentValidation;
namespace Application.Validations.Loans;

public class LoanRequestValidator : AbstractValidator<LoanRequestDtoCreate>
{
    public LoanRequestValidator()
    {
        RuleFor(x => x.CustomerRefId).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Please select customer.")
                                     .NotNull().WithMessage("Please select cusotmer");

        RuleFor(x => x.CurrencyRefId).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Please select currency.")
                                    .GreaterThan(0).WithMessage("Please select currency");

        RuleFor(x => x.AprovedUserId).NotEmpty().WithMessage("Please select Aproved User.");

        RuleFor(x => x.LoanCategoryRefId).GreaterThan(0).WithMessage("Please select Loan Category.");

        RuleFor(x => x.RequestAmount).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Principle must be has value.")
                                     .GreaterThan(0).WithMessage("Principle value must be greater than Zero");
        RuleFor(x => x.AdminFeeRate).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("AdminFee rate must has value.")
                                    .GreaterThan(0).WithMessage("Admin Fee rate must be greater than Zero");
        RuleFor(x => x.InterestRate).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Insterate must has value.")
                                     .GreaterThan(0).WithMessage("Interate must be greater than Zero.");
        RuleFor(x => x.Installments).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Installment number can be blank.")
                                    .GreaterThan(0).WithMessage("Installment must be greater than Zero.");
    }
}
