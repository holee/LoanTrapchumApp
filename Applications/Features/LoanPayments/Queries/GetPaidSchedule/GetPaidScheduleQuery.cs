using ErrorOr;

namespace Applications.Features.LoanPayments.Queries.GetPaidSchedule;

public sealed record GetPaidScheduleQuery(int? BranchId, string? LoanId) : IRequest<ErrorOr<IEnumerable<VSchedule>>>;
