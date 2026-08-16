using Applications.Interfaces.Transactions;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgerTypes;

public sealed class GetLedgerTypesQueryHandler(ILedgerService ledgerService)
    : IRequestHandler<GetLedgerTypesQuery, PageMetaData<LedgerType>>
{
    public async Task<PageMetaData<LedgerType>> Handle(GetLedgerTypesQuery request, CancellationToken cancellationToken)
        => await ledgerService.GetLedgerTypes(request.LedgerTypeId, request.Filter, cancellationToken);
}
