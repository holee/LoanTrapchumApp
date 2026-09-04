using Application.Features.LoanRequests.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddLoanRequestDescription;

public record AddLoanRequestDescriptionCommand(LoanDescriptionsDtoCreate Req)
        : IRequest<ErrorOr<string>>;
