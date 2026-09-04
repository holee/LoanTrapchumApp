using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.DeleteLoanRequestGuarantor;

public record DeleteLoanRequestGuarantorCommand(string? LoanRequestId, int? BranchId, long? GuarantorLinkId)
    : IRequest<ErrorOr<string>>;
