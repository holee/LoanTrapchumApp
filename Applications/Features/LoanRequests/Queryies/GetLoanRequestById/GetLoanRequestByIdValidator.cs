using FluentValidation;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestById;

public sealed class GetLoanRequestByIdValidator : AbstractValidator<GetLoanRequestByIdQuery>
{
    public GetLoanRequestByIdValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("BranchId is required.");
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");
        RuleFor(x => x.LoanRequestId)
            .NotEmpty().WithMessage("LoanRequestId is required.");
    }
}