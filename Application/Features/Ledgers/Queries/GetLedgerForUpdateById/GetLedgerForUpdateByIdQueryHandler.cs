using Application.Features.Ledgers.Dto;
using Application.Interfaces.Transactions;
using ErrorOr;
using Mapster;

namespace Application.Features.Ledgers.Queries.GetLedgerForUpdateById;

public sealed class GetLedgerForUpdateByIdQueryHandler : IRequestHandler<GetLedgerForUpdateByIdQuery, ErrorOr<LedgerUpdateDto>>
{
    private readonly ILedgerService _ledgerService;
    public GetLedgerForUpdateByIdQueryHandler(ILedgerService ledgerService)
    {
        _ledgerService = ledgerService;
    }
    public async Task<ErrorOr<LedgerUpdateDto>> Handle(GetLedgerForUpdateByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _ledgerService.GetLedgerByIdAsync(request.LedgerId, null, cancellationToken);

        if (result is null)
        {
            return Error.NotFound("Ledger.NotFound", $"Ledger with ID {request.LedgerId} not found.");
        }

        return result.Adapt<LedgerUpdateDto>();

    }
}
