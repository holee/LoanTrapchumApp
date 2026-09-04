using Application.Features.Ledgers.Dto;

namespace Application.Features.Ledgers.Queries.GetLedgerByCurrency;

public record GetLedgerByCurrencyQuery(int? LedgerId, int? CurrencyId, QueryParam Query)
              : IRequest<PageMetaData<VLedger>>;
