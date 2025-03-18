using Applications.Requests.Transactions.CashManagements;
using FluentValidation;
namespace Applications.Validations.Transactions
{
    public class TransactionCashVaultDtoCreateValidator : AbstractValidator<TransactionCashVaultDtoCreate>
    {
        public TransactionCashVaultDtoCreateValidator()
        {
            RuleFor(x=>x.UserRefId).NotEmpty().WithMessage("Please Select User.")
                                   .GreaterThan(0).WithMessage("There Is No User Selected");
        }
    }
}
