using Applications.Commons.Messages.Incomes;
using Applications.Features.Incomes.Dto;
using FluentValidation;
namespace Applications.Features.Incomes.Command.CreateIncomeExpenseSource;

public sealed class CreateIncomeExpeseValidator : AbstractValidator<CreateIncomeExpenseDto>
{
    public CreateIncomeExpeseValidator()
    {
        RuleFor(x => x.Description).NotEmpty()
                                  .WithMessage(IncomeExpenseMessages.Required);
        RuleFor(x => x.Types).NotEmpty()
                                  .WithMessage(IncomeExpenseMessages.Types);
    }
}
