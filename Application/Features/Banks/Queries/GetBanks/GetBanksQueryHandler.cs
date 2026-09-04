using Application.Features.Banks.Dtos;
using Application.Interfaces.Banks;
using ErrorOr;

namespace Application.Features.Banks.Queries.GetBanks;

public sealed class GetBanksQueryHandler : IRequestHandler<GetBanksQuery, PageMetaData<BankDto>>
{
    private readonly IBankService _bankService;

    public GetBanksQueryHandler(IBankService bankService)
    {
        _bankService = bankService;
    }

    public async Task<PageMetaData<BankDto>> Handle(GetBanksQuery query, CancellationToken cancellationToken = default)
    {
        return await _bankService.GetAllAsync(query.BranchId, query.Query,cancellationToken);
    }
}
