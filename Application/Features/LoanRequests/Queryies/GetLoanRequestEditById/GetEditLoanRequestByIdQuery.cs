using Application.Responses.ViewModels.Loans.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestById;
 
public record GetEditLoanRequestByIdQuery(int? BranchId, string? CustomerId, string? LoanRequestId, bool Include) : IRequest<ErrorOr<VLoanRequests>>;
