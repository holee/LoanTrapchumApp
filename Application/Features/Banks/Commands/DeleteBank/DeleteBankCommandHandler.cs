using Application.Interfaces.Banks;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Banks.Commands.DeleteBank;

/// <summary>
/// Handler for DeleteBankCommand
/// </summary>
public sealed class DeleteBankCommandHandler : IRequestHandler<DeleteBankCommand, ErrorOr<bool>>
{
    private readonly IBankService _bankService;

    public DeleteBankCommandHandler(IBankService bankService)
    {
        _bankService = bankService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteBankCommand command, CancellationToken cancellationToken = default)
    {
        // Check if bank exists
        var existingBank = await _bankService.GetByIdAsync(command.BankId);
        if (existingBank is null)
        {
            return Error.NotFound("Bank.NotFound", $"Bank with ID {command.BankId} was not found");
        }

        var result = await _bankService.DeleteAsync(command.BankId);

        if (!result)
        {
            return Error.Failure("Bank.DeleteFailed", "Failed to delete bank");
        }

        return true;
    }
}
