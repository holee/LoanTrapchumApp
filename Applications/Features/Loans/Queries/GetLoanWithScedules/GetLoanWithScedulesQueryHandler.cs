using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.Loans.Queries.GetLoanWithScedules;

public sealed class GetLoanWithScedulesQueryHandler(ILoanScheduleRepo scheduleRepo) : IRequestHandler<GetLoanWithScedulesQuery, ErrorOr<IEnumerable<LoanScheduleResponse>>>
{
    public async Task<ErrorOr<IEnumerable<LoanScheduleResponse>>> Handle(GetLoanWithScedulesQuery request, CancellationToken cancellationToken)
    {

        var result = await scheduleRepo.GetLoanWithSchedules(request.BranchId, request.LoanId, cancellationToken);

        if (result.IsSuccess && result.Data is not null)
        {
            return ErrorOrFactory.From(result.Data);
        }

        return Error.NotFound("LoanSchedules.NotFound", "No loan schedules found for the given criteria.");

    }
}