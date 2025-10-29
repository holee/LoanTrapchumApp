using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.CustomerIncomeCommands;

public sealed class CreateCustomerIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, ErrorOr<long>>
{
    private readonly ICustomerIncomeExpendService _cIncomeService;

    public CreateCustomerIncomeCommandHandler(ICustomerIncomeExpendService customerIncomeService)
    {
        _cIncomeService = customerIncomeService;
    }


    public async Task<ErrorOr<long>> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var data = request.ToCreateCommand();

        var rsp = await _cIncomeService.AddOrUpdateIncomeAsync(data);

        return rsp;

    }
}
