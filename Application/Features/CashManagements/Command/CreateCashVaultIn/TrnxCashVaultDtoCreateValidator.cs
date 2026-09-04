using Application.Features.Transactions.CashManagements;

namespace Application.Features.CashManagements.Command.CreateCashVaultIn;

public sealed class TrnxCashVaultDtoCreateValidator : AbstractValidator<TrnxCashVaultDtoCreate>
{
    public TrnxCashVaultDtoCreateValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("BranchId is required.")
            .GreaterThan(0).WithMessage("BranchId must be greater than 0.");

        RuleFor(x => x.TransactionDate).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("TransactionDate is required.")
            .GreaterThan(DateTime.MinValue).WithMessage("TransactionDate must be a valid date.");

        RuleFor(x => x.UserRefId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("UserRefId is required.")
            .NotEmpty().WithMessage("UserRefId must be greater than 0.");

        RuleFor(x => x.TranTypeRefId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("TranTypeRefId is required.")
            .GreaterThan(0).WithMessage("TranTypeRefId must be greater than 0.");


        When(x => x.CashVault != null, () =>
        {
            RuleFor(x => x.CashVault).SetValidator(new CashVaultDtoCreateValidator());
        });


    }
}
