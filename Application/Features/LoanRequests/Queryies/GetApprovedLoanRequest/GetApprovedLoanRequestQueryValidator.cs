using FluentValidation;

namespace Application.Features.LoanRequests.Queryies.GetApprovedLoanRequest;

public sealed class GetApprovedLoanRequestQueryValidator : AbstractValidator<GetApprovedLoanRequestQuery>
{
    public GetApprovedLoanRequestQueryValidator()
    {
        RuleFor(x => x.BranchId)
            .GreaterThan(0).When(x => x.BranchId.HasValue)
            .WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.CustomerId)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.CustomerId))
            .WithMessage("CustomerId cannot be empty.");
        RuleFor(x => x.LoanRequestId)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.LoanRequestId))
            .WithMessage("LoanRequestId cannot be empty.");
    }
}
