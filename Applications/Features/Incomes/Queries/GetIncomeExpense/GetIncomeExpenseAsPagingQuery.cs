using Applications.Features.Incomes.Dto;
using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Incomes.Queries.GetIncomeExpense;

public sealed class GetIncomeExpenseAsPagingQuery : IRequestHandler<GetIncomeExpenseQuery, Pager<IncomeExpenseDto>>
{
    private readonly ICustomerIncomeExpendService _incomeExpenseService;
    public GetIncomeExpenseAsPagingQuery(ICustomerIncomeExpendService incomeExpenseService)
    {
        _incomeExpenseService = incomeExpenseService;
    }
    public async Task<Pager<IncomeExpenseDto>> Handle(GetIncomeExpenseQuery request, CancellationToken cancellationToken)
    {
        return await _incomeExpenseService.GetIncomeExpenseAsPagingAsync(request.Id, request.Type, request.Query, cancellationToken);
    }


}