using Applications.Features.Ledgers.Dto;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Commands.CreateLedger;

public record CreateLedgerCommand(LedgerCreateDto Entity) : IRequest<ErrorOr<VLedger>>;
