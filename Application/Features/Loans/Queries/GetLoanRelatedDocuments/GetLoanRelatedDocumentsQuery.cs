using Application.Features.Loans.Dtos.Responses;

namespace Application.Features.Loans.Queries.GetLoanRelatedDocuments;

public record GetLoanRelatedDocumentsQuery(string? LoanId, int? BranchId) : IRequest<IEnumerable<VLoanDisbursementDocuments>>;