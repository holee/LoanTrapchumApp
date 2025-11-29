using Applications.Enumerations;
using Applications.Features.Loans.Responses;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;

namespace Applications.Features.Loans.Queries.GetLoanRelatedDocuments;

public record GetLoanRelatedDocumentsQuery(string? LoanId, int? BranchId) : IRequest<IEnumerable<VLoanRequestsDocuments>>;