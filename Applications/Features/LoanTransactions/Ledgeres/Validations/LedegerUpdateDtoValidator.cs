using Applications.Features.Ledgers.Dto;
using FluentValidation;
namespace Applications.Validations.Transactions
{
    public sealed class LedegerUpdateDtoValidator : AbstractValidator<LedgerUpdateDto>
    {
        public LedegerUpdateDtoValidator()
        {
            RuleFor(x => x.LedgerName).NotEmpty().WithMessage("please enter leadger name.")
                                       .MaximumLength(150).WithMessage("leger name not to exceed 150 characters");
            RuleFor(x => x.LedgerTypeRefId).NotEmpty().WithMessage("please enter leadger type.")
                                           .GreaterThan(0).WithMessage("please enter ledger type");
            RuleFor(x => x.CurrencyRefId).NotEmpty().WithMessage("please enter leadger type.")
                                         .GreaterThan(0).WithMessage("please enter ledger type");
            RuleFor(x => x.LedgerCode).NotEmpty().WithMessage("please enter leadger name.");
            RuleFor(x => x.ParentRefId).Must((p, id) => ParentMustHasValue(p, id))
                                     .WithMessage("Please select parent ledger type.");
            RuleFor(x => x.BankNumber)
                            .Must((p, bankNumber) => IsCashAndBank(p, bankNumber))
                            .WithMessage("Bank must not blank");
        }

        private bool ParentMustHasValue(LedgerUpdateDto p, int? parentId)
        {
            if (!p.IsMainGl && p.IsSubTypeOf)
                return parentId > 0;
            return true;
        }

        private bool IsCheckedSubTypeOf(LedgerUpdateDto p, bool isSubTypeOf)
        {
            if (!p.IsMainGl && !p.IsSubTypeOf) return false;
            return true;
        }

        private bool IsCashAndBank(LedgerUpdateDto p, string? bankNumber)
        {
            if (p.LedgerTypeRefId == 1)
                return !string.IsNullOrEmpty(bankNumber);
            return true;
        }
    }
}
