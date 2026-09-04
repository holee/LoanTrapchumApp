using Application.Interfaces.Customers;
using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Queries.GetIncomeByCustomerId;



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
