using Applications.Interfaces.Transactions;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgerTypeById;

public sealed class GetLedgerTypeByIdQueryHandler(ILedgerService ledgerService)
    : IRequestHandler<GetLedgerTypeByIdQuery, ErrorOr<LedgerType>>
{
    public async Task<ErrorOr<LedgerType>> Handle(GetLedgerTypeByIdQuery request, CancellationToken cancellationToken)
        => await ledgerService.GetLedgerTypeById(request.LedgerTypeId);
}
