using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestGuarantor;

public record GetLoanRequestGuarantorQuery(int? BranchId, string? LoanRequestId, string? CustomerId)
    : IRequest<ErrorOr<IEnumerable<VLoanRequestGuarantors>>>;
