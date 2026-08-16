using Applications.Features.Ledgers.Dto;

namespace Applications.Features.Ledgers.Queries.GetLedgerByCurrency;

public record GetLedgerByCurrencyQuery(int? LedgerId, int? CurrencyId, QueryParam Query)
              : IRequest<PageMetaData<VLedger>>;
