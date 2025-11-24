using Applications.Interfaces.Customers;
using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetCustomerRequest;

public record GetCustomerRequestQuery(string CustomerId) : IRequest<ErrorOr<IEnumerable<VLoanRequests>>>;

public sealed class GetCustomerRequestQueryHandler : IRequestHandler<GetCustomerRequestQuery, ErrorOr<IEnumerable<VLoanRequests>>>
{

    private readonly ICustomerService _customerService;

    public GetCustomerRequestQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<ErrorOr<IEnumerable<VLoanRequests>>> Handle(GetCustomerRequestQuery request, CancellationToken cancellationToken)
    {
        var rsp = await _customerService.GetCustomerLoanRequestAsync(request.CustomerId, cancellationToken);

        if (rsp.IsSuccess && rsp.Data is not null)
        {
            return ErrorOrFactory.From(rsp.Data);
        }

        return Error.NotFound("LoanRequest.NotFound", "Loan request not found for the specified customer.");

    }
}

