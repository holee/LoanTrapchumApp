using Application.Features.Loans.Dtos.Responses;
using Application.Interfaces.Customers;

namespace Application.Features.Customers.Queries.GetLoanByCustomerId;

public sealed record GetLoanByCustomerIdQueryHandler(ICustomerService _customerService) : IRequestHandler<GetLoanByCustomerIdQuery, Pager<VLoanDisbursments>>
{
    public async Task<Pager<VLoanDisbursments>> Handle(GetLoanByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetCustomerLoanHistoryAsync(request.CustomerId, cancellationToken);
    }
}
