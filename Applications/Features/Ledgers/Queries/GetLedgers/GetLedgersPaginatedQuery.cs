using Applications.Commons;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Ledgers;

namespace Applications.Features.Ledgers.Queries.GetLedgers;

public record GetLedgersPaginatedQuery(int? CurrencyId, QueryParam Param) : IRequest<Pager<VLedger>>;
