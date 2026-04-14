using Applications.Features.Loans.Dtos.Responses;

namespace Applications.Features.Loans.Queries.GetLoanRelatedDocuments;

public record GetLoanRelatedDocumentsQuery(string? LoanId, int? BranchId) : IRequest<IEnumerable<VLoanDisbursementDocuments>>;