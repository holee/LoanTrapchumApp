using Application.Features.Ledgers.Dto;
using Application.Interfaces.Transactions;
using ErrorOr;

namespace Application.Features.Ledgers.Queries.GetLedgerDetailsById; 

public sealed class GetLedgerDetailsByIdQueryHandler(ILedgerService ledgerService)
    : IRequestHandler<GetLedgerDetailsByIdQuery, ErrorOr<VLedger?>>
{
    public async Task<ErrorOr<VLedger?>> Handle(GetLedgerDetailsByIdQuery request, CancellationToken cancellationToken)
    {
		try
		{
            return await ledgerService.GetLedgerByIdAsync(request.LedgerId, cancellationToken, request.columns);

        }
        catch (Exception ex)
		{
			throw new Exception($"An error occurred while retrieving the ledger details: {ex.Message}", ex);
        }
     
        
    }

}
