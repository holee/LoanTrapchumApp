using Applications.Features.Transactions.CashManagements;
using FluentValidation;

namespace Applications.Requests.Validations.Transactions
{
    public class TransactionCashVaultDtoCreateValidator : AbstractValidator<TrnxCashVaultDtoCreate>
    {
        public TransactionCashVaultDtoCreateValidator()
        {
            RuleFor(x => x.UserRefId).NotEmpty().WithMessage("Please Select User.");
        }
    }
}
