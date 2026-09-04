using Application.Features.Incomes.Command.GetEditIncomeExpenseSource;
using Application.Features.Incomes.Dto;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Queries.GetEditIncomeExpenseSource;

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