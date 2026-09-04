using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestDetailById;

public record GetLoanRequestDetailByIdQuery(int? BranchId, string? CustomerId, string? LoanRequestId, bool Include = true)
       : IRequest<ErrorOr<VLoanRequests>>;
