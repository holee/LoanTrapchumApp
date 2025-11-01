using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Queries.GetIncomeByCustomerId;



public sealed class GetIncomeByCustomerQueryHandler : IRequestHandler<GetIncomeByCustomerQuery, ErrorOr<IEnumerable<VCustomerIncomes>>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public GetIncomeByCustomerQueryHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<IEnumerable<VCustomerIncomes>>> Handle(GetIncomeByCustomerQuery request, CancellationToken cancellationToken = default)
    {
        var rsp = await _custInomeSerivce.GetIncomesAsync(request.CustomerId);

        return rsp;
    }
}
