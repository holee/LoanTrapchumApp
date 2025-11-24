using FluentValidation;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestDetailById;

public sealed class GetLoanRequestDetailByIdQueryValidator : AbstractValidator<GetLoanRequestDetailByIdQuery>
{
    public GetLoanRequestDetailByIdQueryValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("BranchId is required.");
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");
        RuleFor(x => x.LoanRequestId)
            .NotEmpty().WithMessage("LoanRequestId is required.");
    }
}
