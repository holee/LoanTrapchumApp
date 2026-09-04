using Application.Features.Banks.Dtos;
using Application.Features.Banks.Mappers;
using Application.Interfaces.Banks;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Banks.Queries.GetBankById;

/// <summary>
/// Handler for GetBankByIdQuery
/// </summary>
public sealed class GetBankByIdQueryHandler : IRequestHandler<GetBankByIdQuery, ErrorOr<BankDto>>
{
    private readonly IBankService _bankService;

    public GetBankByIdQueryHandler(IBankService bankService)
    {
        _bankService = bankService;
    }

    public async Task<ErrorOr<BankDto>> Handle(GetBankByIdQuery query, CancellationToken cancellationToken = default)
    {
        var bank = await _bankService.GetByIdAsync(query.BankId);

        if (bank is null)
        {
            return Error.NotFound("Bank.NotFound", $"Bank with ID {query.BankId} was not found");
        }

        return bank.ToBankDto();
    }
}
