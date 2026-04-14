using Applications.Interfaces.Customers;
using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetCustomerRequest;

public record GetCustomerRequestQuery(string CustomerId) : IRequest<Pager<VLoanRequests>>;

public sealed class GetCustomerRequestQueryHandler : IRequestHandler<GetCustomerRequestQuery, Pager<VLoanRequests>>
{

    private readonly ICustomerService _customerService;

    public GetCustomerRequestQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<Pager<VLoanRequests>> Handle(GetCustomerRequestQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetCustomerLoanRequestAsync(request.CustomerId, cancellationToken);

    }
}

