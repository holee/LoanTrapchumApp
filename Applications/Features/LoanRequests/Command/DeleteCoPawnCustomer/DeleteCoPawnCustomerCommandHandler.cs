using Domains.Models.Loans;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.DeleteCoPawnCustomer;

public sealed class DeleteCoPawnCustomerCommandHandler : IRequestHandler<DeleteCoPawnCustomerCommand, ErrorOr<bool>>
{
    private readonly ILoanRequestService _loanRequestRepository;
    public DeleteCoPawnCustomerCommandHandler(ILoanRequestService loanRequestRepository)
    {
        _loanRequestRepository = loanRequestRepository;
    }
    public async Task<ErrorOr<bool>> Handle(DeleteCoPawnCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var model = new Domains.Models.Loans.CoPawnCustomer
            {
                BranchId = request.CoPawnRequest.BranchId,
                RequestId = request.CoPawnRequest.RequestId,
                CustomerId = request.CoPawnRequest.CustomerId
            };


            var result = await _loanRequestRepository.DeleteCoPawnCustomersAsync(model, cancellationToken);

            if (!result)
            {
                return Error.NotFound(description: "Co-Pawn Customer not found.");
            }

            return result;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }
}