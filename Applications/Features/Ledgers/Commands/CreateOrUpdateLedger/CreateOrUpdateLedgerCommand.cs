using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Commands.CreateOrUpdateLedger;

public record CreateOrUpdateLedgerCommand(Ledger Entity) : IRequest<ErrorOr<string>>;
