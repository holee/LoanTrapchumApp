using Applications.Features.Banks.Dtos;
using Applications.Features.Banks.Mappers;
using Applications.Interfaces.Banks;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Banks.Commands.CreateBank;

public sealed class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, ErrorOr<BankDto>>
{
    private readonly IBankService _bankService;

    public CreateBankCommandHandler(IBankService bankService)
    {
        _bankService = bankService;
    }

    public async Task<ErrorOr<BankDto>> Handle(CreateBankCommand command, CancellationToken cancellationToken = default)
    {
        var bank = command.Request.ToBank();

        var result = await _bankService.CreateAsync(bank);

        if (result is null)
        {
            return Error.Failure("Bank.CreateFailed", "Failed to create bank");
        }

        return result;
    }
}
