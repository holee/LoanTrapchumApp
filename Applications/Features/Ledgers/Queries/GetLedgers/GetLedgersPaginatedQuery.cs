using Applications.Commons;
using Applications.Features.Ledgers.Dto;
using Applications.Responses.Paginations;

namespace Applications.Features.Ledgers.Queries.GetLedgers;

public record GetLedgersPaginatedQuery(int? CurrencyId, QueryParam Param,params string[] Columns) : IRequest<PageMetaData<VLedger>>;
