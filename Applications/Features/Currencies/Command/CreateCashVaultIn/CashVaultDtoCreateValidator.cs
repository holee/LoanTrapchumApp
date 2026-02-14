using Applications.Features.Transactions.CashManagements;

namespace Applications.Features.Currencies.Command.CreateCashVaultIn;

public class CashVaultDtoCreateValidator : AbstractValidator<CashVaultDtoCreate>
{
    public CashVaultDtoCreateValidator()
    {
        RuleFor(x => x.BranchRefId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("CashVaultTypeRefId is required.")
            .GreaterThan(0).WithMessage("CashVaultTypeRefId must be greater than 0.");

        RuleFor(x => x.UserRefId).Cascade(CascadeMode.Stop)
           .NotNull().WithMessage("UserRefId is required.")
           .NotEmpty().WithMessage("UserRefId must be greater than 0.");

        RuleFor(x => x.CvStatusRefId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("CvStatusRefId is required.")
            .GreaterThan(0).WithMessage("CvStatusRefId must be greater than 0.");
    }
}