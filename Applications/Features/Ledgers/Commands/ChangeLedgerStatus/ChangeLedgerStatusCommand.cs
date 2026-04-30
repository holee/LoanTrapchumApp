using Applications.Features.Ledgers.Dto;
using ErrorOr;

namespace Applications.Features.Ledgers.Commands.ChangeLedgerStatus;

public record ChangeLedgerStatusCommand(LedgerChangeStatus Req) : IRequest<ErrorOr<string>>;
