using Applications.Interfaces.Transactions;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgerTypes;

public sealed class GetLedgerTypesQueryHandler(ILedgerService ledgerService)
    : IRequestHandler<GetLedgerTypesQuery, ErrorOr<IEnumerable<LedgerType>>>
{
    public async Task<ErrorOr<IEnumerable<LedgerType>>> Handle(GetLedgerTypesQuery request, CancellationToken cancellationToken)
        => await ledgerService.FindLedgerTypes(request.LedgerTypeId, request.Filter);
}
