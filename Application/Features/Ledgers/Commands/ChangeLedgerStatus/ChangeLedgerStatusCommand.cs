using Application.Features.Ledgers.Dto;
using ErrorOr;

namespace Application.Features.Ledgers.Commands.ChangeLedgerStatus;

public record ChangeLedgerStatusCommand(LedgerChangeStatus Req) : IRequest<ErrorOr<string>>;
