using Applications.Requests.Transactions.Journals;
using FluentValidation;

namespace Applications.Requests.Validations.Transactions
{
    public class TrnxJournalDtoCreateValidtor : AbstractValidator<TrnxJournalDtoCreate>
    {
        public TrnxJournalDtoCreateValidtor()
        {
            RuleFor(x => x.TransactionTypeName).NotEmpty()
                                               .WithMessage("Transaction Name is required.");

        }
    }
}
