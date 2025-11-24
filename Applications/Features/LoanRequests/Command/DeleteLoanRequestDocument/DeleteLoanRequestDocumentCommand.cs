using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.DeleteLoanRequestDocument;

public record DeleteLoanRequestDocumentCommand(string? LoanRequestId, int? BranchId, long? DocumentId) : IRequest<ErrorOr<string>>;
