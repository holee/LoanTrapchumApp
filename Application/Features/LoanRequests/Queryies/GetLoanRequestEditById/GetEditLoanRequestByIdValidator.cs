using FluentValidation;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestById;

public sealed class GetEditLoanRequestByIdValidator : AbstractValidator<GetLoanRequestByIdQuery>
{
    public GetEditLoanRequestByIdValidator() 
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("BranchId is required.");
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");
        RuleFor(x => x.LoanRequestId)
            .NotEmpty().WithMessage("LoanRequestId is required.");
    }
}