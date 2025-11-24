using Applications.Features.LoanRequests.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.AddLoanRequestGuarantor;

public record AddLoanRequestGuarantorCommand(LoanRequestGuarantorsDtoCreate Request) : IRequest<ErrorOr<string>>;
