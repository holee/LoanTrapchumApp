using Applications.Features.Ledgers.Dto;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgers;

public record GetLedgersQuery(int? LedgerId, int? CurrencyId, string? Filter) : IRequest<ErrorOr<IEnumerable<VLedger>>>;
