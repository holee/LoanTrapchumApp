using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Customers;

namespace Applications.Features.Customers.Queries.GetLoanByCustomerId;

public sealed record GetLoanByCustomerIdQueryHandler(ICustomerService _customerService) : IRequestHandler<GetLoanByCustomerIdQuery, Pager<VLoanDisbursments>>
{
    public async Task<Pager<VLoanDisbursments>> Handle(GetLoanByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetCustomerLoanHistoryAsync(request.CustomerId, cancellationToken);
    }
}
