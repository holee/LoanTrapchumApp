using Application.Interfaces.Transactions;
using ErrorOr;

namespace Application.Features.Ledgers.Queries.GetLedgers;

//public sealed class GetLedgersQueryHandler(ILedgerService ledgerService)
//    : IRequestHandler<GetLedgersQuery, ErrorOr<IEnumerable<VLedger>>>
//{
//    public async Task<ErrorOr<IEnumerable<VLedger>>> Handle(GetLedgersQuery request, CancellationToken cancellationToken)
//        => await ledgerService.GetLedgersAsync(request.LedgerId, request.CurrencyId, request.Filter);
//}
