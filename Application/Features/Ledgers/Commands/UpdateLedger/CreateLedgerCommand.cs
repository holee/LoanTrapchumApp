using Application.Features.Ledgers.Dto;
using Domains.Models.Journals;
using ErrorOr;

namespace Application.Features.Ledgers.Commands.UpdateLedger; 

public record UpdateLedgerCommand(LedgerUpdateDto Entity) : IRequest<ErrorOr<VLedger>>;
