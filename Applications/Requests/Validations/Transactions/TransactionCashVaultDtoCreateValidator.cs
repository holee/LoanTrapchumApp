using Applications.Requests.Transactions.CashManagements;
using FluentValidation;

namespace Applications.Requests.Validations.Transactions
{
    public class TransactionCashVaultDtoCreateValidator : AbstractValidator<TransactionCashVaultDtoCreate>
    {
        public TransactionCashVaultDtoCreateValidator()
        {
            RuleFor(x => x.UserRefId).NotEmpty().WithMessage("Please Select User.");
        }
    }
}
