using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Utils;
using ErrorOr;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.LoanRequests.Command.VerifiedLoanRequest;

public record VerifiedLoanRequestCommand(RequestDtoBaseCreate Request) : IRequest<ErrorOr<RequestResult>>;

public class VerifiedLoanRequestCommandHandler(ILoanRequestService reqService) : IRequestHandler<VerifiedLoanRequestCommand, ErrorOr<RequestResult>>
{
    public async Task<ErrorOr<RequestResult>> Handle(VerifiedLoanRequestCommand command, CancellationToken cancellationToken)
    {
        return await reqService.VerifiedLoanRequestAsync(command.Request);
    }
}

public sealed class VerifiedLoanRequestCommandValidator : AbstractValidator<VerifiedLoanRequestCommand>
{
    public VerifiedLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request.LoanRequestId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Loan Request Id cannot be null.")
            .NotEmpty().WithMessage("Loan Request Id is required.");
        RuleFor(x => x.Request.CustomerId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Customer Id cannot be null.")
            .NotEmpty().WithMessage("Customer Id is required.");
        RuleFor(x => x.Request.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Branch Id cannot be null.")
            .GreaterThan(0).WithMessage("Branch Id must be greater than zero.");
    }
}
