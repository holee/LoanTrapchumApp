using Application.Responses.ViewModels.Loans.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestsDocuments;

public record GetLoanRequestDocumentQuery(string? LoanRequestId, int? BranchId) : IRequest<ErrorOr<IEnumerable<VLoanRequestsDocuments>>>;
