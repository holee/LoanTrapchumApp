using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgerTypes;

public record GetLedgerTypesQuery(int? LedgerTypeId, string? Filter) : IRequest<ErrorOr<IEnumerable<LedgerType>>>;
