using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.DeleteLoanRequestGuarantor;

public record DeleteLoanRequestGuarantorCommand(string? LoanRequestId, int? BranchId, long? GuarantorLinkId)
    : IRequest<ErrorOr<string>>;
