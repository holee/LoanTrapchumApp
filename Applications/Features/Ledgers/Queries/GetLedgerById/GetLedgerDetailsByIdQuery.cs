using Applications.Features.Ledgers.Dto;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgerById;

public record GetLedgerByIdQuery(int? LedgerId, params string[] columns) : IRequest<ErrorOr<Ledger?>>;
