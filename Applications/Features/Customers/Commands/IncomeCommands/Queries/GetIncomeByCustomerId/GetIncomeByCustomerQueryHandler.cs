using Applications.Commons.Messages.Incomes;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.Queries.GetIncomeByCustomerId;



public sealed class GetIncomeByCustomerQueryHandler : IRequestHandler<GetIncomeByCustomerQuery, ErrorOr<IEnumerable<VCustomerIncomes>>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public GetIncomeByCustomerQueryHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<IEnumerable<VCustomerIncomes>>> Handle(GetIncomeByCustomerQuery request, CancellationToken cancellationToken = default)
    {
        var rsp = await _custInomeSerivce.GetIncomes(request.CustomerId);

        return rsp;
    }
}
