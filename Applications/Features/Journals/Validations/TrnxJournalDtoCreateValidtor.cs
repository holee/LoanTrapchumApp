using FluentValidation;

namespace Applications.Features.Journals.Validations
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
