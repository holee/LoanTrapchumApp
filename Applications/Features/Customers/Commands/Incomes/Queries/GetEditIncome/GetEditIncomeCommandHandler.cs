using Applications.Interfaces.Customers;
using Applications.Utils;
using Domains.Models.Lists.Customers;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Incomes.Queries.GetEditIncome;

public sealed class GetEditIncomeCommandHandler : IRequestHandler<GetEditIncomeCommand,ErrorOr<CustomerIncome>>
{

    private readonly ICustomerIncomeExpendService _cuscomeService;

    public GetEditIncomeCommandHandler(ICustomerIncomeExpendService cuscomeService)
    {
        _cuscomeService = cuscomeService;
    }

    public async Task<ErrorOr<CustomerIncome>> Handle(GetEditIncomeCommand request, CancellationToken cancellationToken = default)
    {
        return await _cuscomeService.GetEditIncomeAsync(request.Id, request.CustomerId);
    }
}
