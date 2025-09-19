using Applications.Features.Transactions.Journals;
using FluentValidation;
namespace Applications.Validations.Transactions 
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
