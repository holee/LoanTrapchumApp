using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.Loans.Queries.GetLoanWithScedules;

public sealed class GetLoanWithScedulesQueryHandler(IScheduleService scheduleRepo) : IRequestHandler<GetLoanWithScedulesQuery, ErrorOr<IEnumerable<LoanResponse>>>
{
    public async Task<ErrorOr<IEnumerable<LoanResponse>>> Handle(GetLoanWithScedulesQuery request, CancellationToken cancellationToken)
    {

        return await scheduleRepo.GetLoanWithSchedules(request.BranchId, request.LoanId, cancellationToken);
    }
}