using FluentValidation;

namespace Application.Features.Incomes.Command.EditIncome
{
    public sealed class EditIncomeCommandValidator : AbstractValidator<EditIncomeCommand>
    {
        public EditIncomeCommandValidator()
        {
            RuleFor(x => x.Request).NotNull().WithMessage("Income data must be provided.");

            When(x => x.Request is not null, () =>
            {
                RuleFor(x => x.Request.Id).GreaterThan(0)
                                    .WithMessage("Income Id is greater than zero.");
                RuleFor(x => x.Request.Amount).GreaterThan(0)
                                   .WithMessage("Income Amount source is greater than zero.");
                RuleFor(x => x.Request.IncomeSource).NotEmpty()
                                    .WithMessage("Income source is required");
            });
           
        }
    }
}
