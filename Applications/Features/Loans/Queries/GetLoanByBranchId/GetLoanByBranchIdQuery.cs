using Applications.Enumerations;
using Applications.Features.Loans.Dtos.Responses;

namespace Applications.Features.Loans.Queries.GetLoanByBranchId;

public record GetLoanByBranchIdQuery(int? BranchId, string? UserId, LoanStatuses State,
    DateTime? Start, DateTime? To, QueryParam Param) : IRequest<Pager<VLoanDisbursments>>;
