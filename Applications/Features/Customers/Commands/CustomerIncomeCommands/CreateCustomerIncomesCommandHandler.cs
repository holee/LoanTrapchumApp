using Applications.Exceptions;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.CustomerIncomeCommands;

public sealed class CreateCustomerIncomesCommandHandler : IRequestHandler<List<CreateIncomeCommand>, ErrorOr<string>>
{
    private readonly ICustomerIncomeExpendService _cIncomeService;

    public CreateCustomerIncomesCommandHandler(ICustomerIncomeExpendService customerIncomeService)
    {
        _cIncomeService = customerIncomeService;
    }


    public async Task<ErrorOr<string>> Handle(List<CreateIncomeCommand> requests, CancellationToken cancellationToken)
    {

        var data = requests.Select(request => request.ToCreateCommand()).ToList();
       
        var customerId = requests.FirstOrDefault()?.CustomerRefId ?? "";

        if (customerId is null)
        {
            throw new NotFoundExceptions("CustomerRefId is required");
        }

        return await _cIncomeService.AddOrUpdateIncomesAsync(customerId, data);
        
    }
}
