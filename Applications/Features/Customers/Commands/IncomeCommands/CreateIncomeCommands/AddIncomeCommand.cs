using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.CreateIncomeCommands;

public sealed record AddIncomeCommand(string CustomerId, IncomeDtoCreate Request) : IRequest<ErrorOr<string>>;

