using Application.Features.Incomes.Dto;
using Application.Interfaces.Customers;
using Application.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Application.Features.Incomes.Queries.GetEditIncome;

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
