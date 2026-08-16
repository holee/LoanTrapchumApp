using Applications.Features.Ledgers.Dto;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Commands.UpdateLedger; 

public record UpdateLedgerCommand(LedgerUpdateDto Entity) : IRequest<ErrorOr<VLedger>>;
