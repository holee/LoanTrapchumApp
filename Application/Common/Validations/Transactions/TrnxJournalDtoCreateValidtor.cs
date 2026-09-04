using FluentValidation;
namespace Application.Validations.Transactions 
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
