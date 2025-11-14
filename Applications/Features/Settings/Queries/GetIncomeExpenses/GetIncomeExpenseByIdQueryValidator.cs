using Applications.Commons.Messages.Settings;
using FluentValidation;

namespace Applications.Features.Settings.Queries.GetIncomeExpenses;

public sealed class GetIncomeExpenseByIdQueryValidator : AbstractValidator<GetIncomeExpenseByIdQuery>
{
    public GetIncomeExpenseByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage(SettingMessage.IdNull)
            .GreaterThan(0)
            .WithMessage(SettingMessage.IdGreaterThanZero);
    }
}