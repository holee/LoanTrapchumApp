using Application.Interfaces.Loans;
using Application.Utils;
using Domains.Models.Loans;
using ErrorOr;
using Mapster;

namespace Application.Features.LoanRequests.Command.AddLoanRequestDocument;

public sealed class AddLoanRequestDocumentsCommandHandler : IRequestHandler<AddLoanRequestDocumentsCommand, ErrorOr<string>>
{
    private readonly ILoanRequestService _loanRequestService;
    public AddLoanRequestDocumentsCommandHandler(ILoanRequestService loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }
    public async Task<ErrorOr<string>> Handle(AddLoanRequestDocumentsCommand request, CancellationToken cancellationToken)
    {
        
        var model = request.Requests.Adapt<List<LoanDocuments>>();
      
        return await _loanRequestService.AddLoanRequestDocumentAsync(model);
    }
}
