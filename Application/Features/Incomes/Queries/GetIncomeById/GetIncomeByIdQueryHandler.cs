using Application.Interfaces.Customers;
using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Queries.GetIncomeById;

public sealed class GetIncomeByIdQueryHandler : IRequestHandler<GetIncomeByIdQuery, ErrorOr<VCustomerIncomes>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public GetIncomeByIdQueryHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<VCustomerIncomes>> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken = default)
    {

        var rsp = await _custInomeSerivce.GetIncomeByIdAsync(request.Id, request.CustomerId);

        return rsp;
    }
}
