using Application.Commons;
using Application.Features.Ledgers.Dto;
using Application.Responses.Paginations;

namespace Application.Features.Ledgers.Queries.GetLedgers;

public record GetLedgersPaginatedQuery(int? CurrencyId, QueryParam Param,params string[] Columns) : IRequest<PageMetaData<VLedger>>;
