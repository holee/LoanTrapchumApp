using Applications.Features.LoanRequests.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.AddLoanRequestDescription;

public record AddLoanRequestDescriptionCommand(LoanDescriptionsDtoCreate Req)
        : IRequest<ErrorOr<string>>;
