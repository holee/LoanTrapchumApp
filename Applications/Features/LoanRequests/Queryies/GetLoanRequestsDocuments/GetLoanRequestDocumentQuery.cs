using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestsDocuments;

public record GetLoanRequestDocumentQuery(string? LoanRequestId, int? BranchId) : IRequest<ErrorOr<IEnumerable<VLoanRequestsDocuments>>>;
