using Applications.Features.Transactions.CashManagements;
namespace Applications.Validations.Transactions
{
    public class TransactionCashVaultDtoCreateValidator : AbstractValidator<TrnxCashVaultDtoCreate>
    {
        public TransactionCashVaultDtoCreateValidator()
        {
            RuleFor(x => x.UserRefId).NotEmpty().WithMessage("Please Select User.");
        }
    }
}
