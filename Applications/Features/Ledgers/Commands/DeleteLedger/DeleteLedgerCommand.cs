using ErrorOr;

namespace Applications.Features.Ledgers.Commands.DeleteLedger;

public record DeleteLedgerCommand(int? Id) : IRequest<ErrorOr<string>>;
