using Application.Features.Dashbaord.Managers.Dto;
using Application.Interfaces;
using ErrorOr;

namespace Application.Features.Dashbaord.Managers.Queries.GetDbManagerLoanSummary;

public sealed class GetDbManagerLoanSummaryQueryValidator : AbstractValidator<GetDbManagerLoanSummaryQuery>
{
    public GetDbManagerLoanSummaryQueryValidator()
    {
        RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
    }
}