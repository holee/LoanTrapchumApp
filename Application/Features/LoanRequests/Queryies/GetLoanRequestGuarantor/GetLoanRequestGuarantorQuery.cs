using Application.Responses.ViewModels.Loans.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestGuarantor;

public record GetLoanRequestGuarantorQuery(int? BranchId, string? LoanRequestId, string? CustomerId)
    : IRequest<ErrorOr<IEnumerable<VLoanRequestGuarantors>>>;
