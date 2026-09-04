using Application.Features.Banks.Dtos;
using Application.Features.Banks.Mappers;
using Application.Interfaces.Banks;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Banks.Commands.UpdateBank;

/// <summary>
/// Handler for UpdateBankCommand
/// </summary>
public sealed class UpdateBankCommandHandler : IRequestHandler<UpdateBankCommand, ErrorOr<BankDto>>
{
    private readonly IBankService _bankService;

    public UpdateBankCommandHandler(IBankService bankService)
    {
        _bankService = bankService;
    }

    public async Task<ErrorOr<BankDto>> Handle(UpdateBankCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var bank = command.Request.ToBank();

            var result = await _bankService.UpdateAsync(command.BankId, bank, cancellationToken);

            if(result is null)
            {
                return Error.NotFound("Bank.NotFound", $"Bank with ID {command.BankId} was not found.");
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the bank.", ex);
        }

    }
}
