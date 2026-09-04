using Application.Interfaces.Customers;
using Application.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Application.Features.Settings.Queries.GetIncomeExpenses;

public sealed class GetIncomeExpenseByIdQueryHandler : IRequestHandler<GetIncomeExpenseByIdQuery,ErrorOr<IncomeExpense>>
{
    private readonly ICustomerIncomeExpendService _service;

    public GetIncomeExpenseByIdQueryHandler(ICustomerIncomeExpendService service)
    {
        _service = service;
    }

    public async Task<ErrorOr<IncomeExpense>> Handle(GetIncomeExpenseByIdQuery request, CancellationToken cancellationToken = default)
    {
        return await _service.GetIncomeExpenseAsync(request.Id, cancellationToken);
    }
}