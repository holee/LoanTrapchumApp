using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.DeleteLoanRequestDocument;

public record DeleteLoanRequestDocumentCommand(string? LoanRequestId, int? BranchId, long? DocumentId) : IRequest<ErrorOr<string>>;
