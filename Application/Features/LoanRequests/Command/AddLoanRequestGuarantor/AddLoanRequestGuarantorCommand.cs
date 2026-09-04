using Application.Features.LoanRequests.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddLoanRequestGuarantor;

public record AddLoanRequestGuarantorCommand(LoanRequestGuarantorsDtoCreate Request) : IRequest<ErrorOr<string>>;
