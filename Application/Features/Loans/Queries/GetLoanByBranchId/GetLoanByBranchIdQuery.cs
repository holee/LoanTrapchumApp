using Application.Enumerations;
using Application.Features.Loans.Dtos.Responses;

namespace Application.Features.Loans.Queries.GetLoanByBranchId;

public record GetLoanByBranchIdQuery(int? BranchId, string? UserId, LoanStatuses State,
    DateTime? Start, DateTime? To, QueryParam Param) : IRequest<Pager<VLoanDisbursments>>;
