using Applications.Features.Ledgers.Dto;
using FluentValidation;

namespace Applications.Requests.Validations.Ledgeres
{
    public sealed class LedgerValidatorCreate : AbstractValidator<LedgerCreateDto>
    {
        public LedgerValidatorCreate()
        {
            RuleFor(x => x.LedgerTypeRefId).GreaterThan(0)
                                          .WithMessage("Please select ledger type.");
            RuleFor(x => x.LedgerName).NotEmpty()
                                    .WithMessage("Ledger Name must not empty.");

            RuleFor(x => x.LedgerCode).NotEmpty()
                                  .WithMessage("Ledger Name must not empty.");

        }

        private bool MustParentId(int d,bool v)
        {
            return true;
        }
    }
}
