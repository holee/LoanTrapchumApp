using Application.Features.Transactions.CashManagements;
using FluentValidation;

namespace Application.Requests.Validations.Transactions
{
    public class TransactionCashVaultDtoCreateValidator : AbstractValidator<TrnxCashVaultDtoCreate>
    {
        public TransactionCashVaultDtoCreateValidator()
        {
            RuleFor(x => x.UserRefId).NotEmpty().WithMessage("Please Select User.");
        }
    }
}
