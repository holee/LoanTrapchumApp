using Applications.Features.Incomes.Command.GetEditIncomeExpenseSource;
using Applications.Features.Incomes.Dto;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Queries.GetEditIncomeExpenseSource;

public sealed class GetEditIncomeExpenseQueryHandler : IRequestHandler<GetEditIncomeExpenseQuery, ErrorOr<IncomeExpenseDto>>
{


    private readonly ICustomerIncomeExpendService _expendService;

    public GetEditIncomeExpenseQueryHandler(ICustomerIncomeExpendService expendService)  
    {
        _expendService = expendService;
    }

    public Task<ErrorOr<IncomeExpenseDto>> Handle(GetEditIncomeExpenseQuery request, CancellationToken cancellationToken = default)
    {
        return _expendService.GetEditIncomeExpenseAsync(request.Id,cancellationToken);
    }
}