using Application.Features.Ledgers.Dto;
using Domains.Models.Journals;
using ErrorOr;

namespace Application.Features.Ledgers.Commands.CreateLedger;

public record CreateLedgerCommand(LedgerCreateDto Entity) : IRequest<ErrorOr<VLedger>>;
