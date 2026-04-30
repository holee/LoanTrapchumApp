using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgerById;

public record GetLedgerByIdQuery(int? LedgerId) : IRequest<ErrorOr<Ledger>>;
