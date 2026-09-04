using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.DeleteLoanRequestDocument;

public sealed class DeleteLoanRequestDocumentCommandHandler(ILoanRequestService _loanRequestService) : IRequestHandler<DeleteLoanRequestDocumentCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteLoanRequestDocumentCommand request, CancellationToken cancellationToken)
    {
        var result = await _loanRequestService.DeleteLoanRequestDocumentAsync(request.LoanRequestId, request.BranchId, request.DocumentId);
        return result;
    }
}
