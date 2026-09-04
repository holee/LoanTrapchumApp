using Application.Features.LoanRequests.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddDescription;

public record AddDescriptionCommand(LoanDescriptionsDtoCreate Request) : IRequest<ErrorOr<string>>;
