namespace Applications.Features.Ledgers.Queries.GetLedgerTypes;

public record GetLedgerTypesQuery(int? LedgerTypeId, QueryParam Filter) : IRequest<PageMetaData<LedgerType>>;
