using FluentValidation;

namespace Applications.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;

public sealed class GetLoanSchedulesByLoanIdQueryValidator : AbstractValidator<GetLoanSchedulesByLoanIdAsPagingQuery>
{
    public GetLoanSchedulesByLoanIdQueryValidator() 
    {
        RuleFor(x => x.LoanId)
            .NotEmpty().WithMessage("LoanId is required.");
        RuleFor(x => x.BranchId)
            .NotNull().WithMessage("BranchId is required.")
            .GreaterThan(0).WithMessage("BranchId must be greater than zero.");
    }
}