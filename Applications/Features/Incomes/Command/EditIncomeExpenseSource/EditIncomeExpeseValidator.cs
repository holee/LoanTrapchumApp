using Applications.Commons.Messages.Incomes;
using Applications.Features.Incomes.Dto;
using FluentValidation;
namespace Applications.Features.Incomes.Command.CreateIncomeExpenseSource;

public sealed class EditIncomeExpeseValidator : AbstractValidator<CreateIncomeExpenseDto>
{
    public EditIncomeExpeseValidator()
    {

        //RuleFor(x => x.Id).GreaterThan(0)
        //                   .WithMessage("ID must be greater than zero.");
        RuleFor(x => x.Description).NotEmpty()
                                  .WithMessage(IncomeExpenseMessages.Required);
        RuleFor(x => x.Types).NotEmpty()
                                  .WithMessage(IncomeExpenseMessages.Types);
    }
}
