using Applications.Commons.Mappers.LoanRequests;
using Applications.Features.LoanRequests.Dtos;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.AddCoPawnCustomers;

public sealed class AddCoPawnCustomersCommandHandler : IRequestHandler<AddCoPawnCustomersCommand, ErrorOr<IEnumerable<CoPawnCustomerDto?>>>
{
    private readonly ILoanRequestService _loanRequestRepository;
    public AddCoPawnCustomersCommandHandler(ILoanRequestService loanRequestRepository)
    {
        _loanRequestRepository = loanRequestRepository;
    }
    public async Task<ErrorOr<IEnumerable<CoPawnCustomerDto?>>> Handle(AddCoPawnCustomersCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var model = request.Request.MapFromCoPawnsDtoCreate();

            var coPawnCustomers = await _loanRequestRepository.AddCoPawnCustomers2Async(request.BranchId, request.RequestId, model, cancellationToken);

            if (coPawnCustomers is null)
            {
                return Error.NotFound(description: "Co-Pawn Customer not found.");
            }

            return coPawnCustomers.ToList();
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }

    }
}