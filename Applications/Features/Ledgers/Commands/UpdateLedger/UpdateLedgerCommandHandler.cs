using Applications.Features.Ledgers.Dto;
using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Ledgers.Commands.UpdateLedger; 

public sealed class UpdateLedgerCommandHandler(ILedgerService ledgerService)
    : IRequestHandler<UpdateLedgerCommand, ErrorOr<VLedger>> 
{
    public async Task<ErrorOr<VLedger>> Handle(UpdateLedgerCommand request, CancellationToken cancellationToken)
    {
        var model = request.Entity.MapFromLedgerDtoUpdate();

        return await ledgerService.CreateOrUpdateLedger(model);
    }
}
