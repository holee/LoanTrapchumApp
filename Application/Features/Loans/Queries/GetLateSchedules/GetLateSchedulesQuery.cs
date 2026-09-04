namespace Application.Features.Loans.Queries.GetLateSchedules;

public record GetLateSchedulesQuery(int? BranchId, string? LoanId, DateTimeOffset? PaymentDate)
    : IRequest<Pager<VSchedule>>;
