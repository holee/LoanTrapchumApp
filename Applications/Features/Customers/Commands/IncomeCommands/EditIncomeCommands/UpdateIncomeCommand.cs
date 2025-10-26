using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.EditIncomeCommands;

public sealed record UpdateIncomeCommand(string CustomerId, IncomeDtoUpdate Request) : IRequest<ErrorOr<string>>;

public sealed record UpdateIncomesCommand(string CustomerId, List<IncomeDtoUpdate> Requests) : IRequest<ErrorOr<string>>;
 