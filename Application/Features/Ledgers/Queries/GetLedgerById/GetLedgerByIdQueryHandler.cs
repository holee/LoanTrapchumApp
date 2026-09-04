using Application.Features.Ledgers.Dto;
using Application.Interfaces.Transactions;
using Domains.Models.Journals;
using ErrorOr;

namespace Application.Features.Ledgers.Queries.GetLedgerById;

public sealed class GetLedgerByIdQueryHandler(ILedgerService ledgerService) : IRequestHandler<GetLedgerByIdQuery, ErrorOr<Ledger?>>
{
    public async Task<ErrorOr<Ledger?>> Handle(GetLedgerByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await ledgerService.GetLedgerByIdAsync(request.LedgerId, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the ledger details: {ex.Message}", ex);
        }


    }

}
