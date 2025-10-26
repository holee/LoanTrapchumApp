using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.CreateIncomeCommands;

public sealed record AddIncomesCommand(string CustomerId, List<IncomeDtoCreate> Request) : IRequest<ErrorOr<string>>;
