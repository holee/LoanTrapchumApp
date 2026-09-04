using Application.Responses.ViewModels.Loans.Schedules;

namespace Application.Features.Loans.Queries.GetLoanSchedulesByPaymentDate;

public record GetLoanSchedulesByPaymentDateQuery(int? BranchId, DateTimeOffset? PaymentDate, bool IsIncludeLate = true, bool IsLateOnly = false, QueryParam? Param = null)
        : IRequest<Pager<VSchedule>>;
