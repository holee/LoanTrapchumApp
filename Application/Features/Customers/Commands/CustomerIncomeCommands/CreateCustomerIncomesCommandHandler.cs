using Application.Exceptions;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Customers.Commands.CustomerIncomeCommands;

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
