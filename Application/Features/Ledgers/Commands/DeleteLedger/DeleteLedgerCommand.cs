using ErrorOr;

namespace Application.Features.Ledgers.Commands.DeleteLedger;

public record DeleteLedgerCommand(int? Id) : IRequest<ErrorOr<string>>;
