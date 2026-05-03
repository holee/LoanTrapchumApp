using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.IsCoPawnExist;

public sealed class IsCoPawnExistQueryHandler : IRequestHandler<IsCoPawnExistQuery, ErrorOr<bool>>
{
    private readonly ILoanRequestService _loanRequestRepository;
    public IsCoPawnExistQueryHandler(ILoanRequestService loanRequestRepository)
    {
        _loanRequestRepository = loanRequestRepository;
    }
    public async Task<ErrorOr<bool>> Handle(IsCoPawnExistQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var isCoPawnExist = await _loanRequestRepository.IsCoPawnExistAsync(request.BranchId, request.CustomerId, cancellationToken);

            return isCoPawnExist;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }
}
