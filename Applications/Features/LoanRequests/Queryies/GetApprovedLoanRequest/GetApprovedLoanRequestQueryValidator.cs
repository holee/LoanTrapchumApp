using FluentValidation;

namespace Applications.Features.LoanRequests.Queryies.GetApprovedLoanRequest;

public sealed class GetApprovedLoanRequestQueryValidator : AbstractValidator<GetApprovedLoanRequestQuery>
{
    public GetApprovedLoanRequestQueryValidator()
    {
        RuleFor(x => x.branchId)
            .GreaterThan(0).When(x => x.branchId.HasValue)
            .WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.customerId)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.customerId))
            .WithMessage("CustomerId cannot be empty.");
        RuleFor(x => x.loanRequestId)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.loanRequestId))
            .WithMessage("LoanRequestId cannot be empty.");
    }
}
