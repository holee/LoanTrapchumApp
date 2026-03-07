using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.Dashbaord.CreditOfficers.Queries.GetNumberLoanRequest;

public sealed class GetNumberLoanRequestQueryHandler(IDashboardService dashboardService) : IRequestHandler<GetNumberLoanRequestQuery, ErrorOr<NumerLoanRequest>>
{
    public async Task<ErrorOr<NumerLoanRequest>> Handle(GetNumberLoanRequestQuery request, CancellationToken cancellationToken)
    {
        return await dashboardService.GetNumberLoanRequestByCo(request.BranchId, request.UserId, cancellationToken);
    }
}
