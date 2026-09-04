using Application.Commons.Messages.Incomes;
using Application.Features.Incomes.Dto;
using FluentValidation;
namespace Application.Features.Incomes.Command.CreateIncomeExpenseSource;

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
