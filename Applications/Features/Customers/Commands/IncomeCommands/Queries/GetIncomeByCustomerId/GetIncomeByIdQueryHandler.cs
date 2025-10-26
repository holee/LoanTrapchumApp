using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.Queries.GetIncomeByCustomerId;

public sealed class GetIncomeByIdQueryHandler : IRequestHandler<GetIncomeByIdQuery, ErrorOr<VCustomerIncomes>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public GetIncomeByIdQueryHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<VCustomerIncomes>> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken = default)
    {

        var rsp = await _custInomeSerivce.GetIncome(request.Id, request.CustomerId);

        return rsp;
    }
}
