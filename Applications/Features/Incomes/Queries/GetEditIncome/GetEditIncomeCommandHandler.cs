using Applications.Features.Incomes.Dto;
using Applications.Interfaces.Customers;
using Applications.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Features.Incomes.Queries.GetEditIncome;

public sealed class GetEditIncomeCommandHandler : IRequestHandler<GetEditIncomeCommand,ErrorOr<EditIncomeDto>>
{

    private readonly ICustomerIncomeExpendService _cuscomeService;

    public GetEditIncomeCommandHandler(ICustomerIncomeExpendService cuscomeService)
    {
        _cuscomeService = cuscomeService;
    }

    public async Task<ErrorOr<EditIncomeDto>> Handle(GetEditIncomeCommand request, CancellationToken cancellationToken = default)
    {
        return await _cuscomeService.GetEditIncomeAsync(request.Id, request.CustomerId);
    }
}
