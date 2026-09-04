using Application.Features.Ledgers.Dto;
using Application.Interfaces.Transactions;
using ErrorOr;

namespace Application.Features.Ledgers.Commands.CreateLedger;

public sealed class CreateLedgerCommandHandler(ILedgerService ledgerService) 
    : IRequestHandler<CreateLedgerCommand, ErrorOr<VLedger>>
{
    public async Task<ErrorOr<VLedger>> Handle(CreateLedgerCommand request, CancellationToken cancellationToken)
    {
        var model = request.Entity.MapFromLedgerDtoCreate();

        return await ledgerService.CreateOrUpdateLedger(model);
    }
}
