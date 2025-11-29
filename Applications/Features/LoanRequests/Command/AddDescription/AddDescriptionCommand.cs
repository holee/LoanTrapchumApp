using Applications.Features.LoanRequests.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.AddDescription;

public record AddDescriptionCommand(LoanDescriptionsDtoCreate Request) : IRequest<ErrorOr<string>>;
