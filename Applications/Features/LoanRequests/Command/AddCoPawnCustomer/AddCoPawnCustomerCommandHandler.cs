using Applications.Commons.Mappers.LoanRequests;
using Applications.Features.LoanRequests.Dtos;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.AddCoPawnCustomer;

public sealed class AddCoPawnCustomerCommandHandler : IRequestHandler<AddCoPawnCustomerCommand, ErrorOr<CoPawnCustomerDto?>>
{
    private readonly ILoanRequestService _loanRequestRepository;
    public AddCoPawnCustomerCommandHandler(ILoanRequestService loanRequestRepository)
    {
        _loanRequestRepository = loanRequestRepository;
    }
    public async Task<ErrorOr<CoPawnCustomerDto?>> Handle(AddCoPawnCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var model = request.Request.MapFromCoPawnDtoCreate();

            var coPawnCustomer = await _loanRequestRepository.AddCoPawnCustomerAsync(model, cancellationToken);

            if (coPawnCustomer is null)
            {
                return Error.NotFound(description: "Co-Pawn Customer not found.");
            }
            return coPawnCustomer;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }

    }
}