using Applications.Exceptions;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using Domains.Models.Lists.Customers;

namespace Applications.Features.Customers.Commands.CustomerIncomeCommands;

public sealed class CreateCustomerIncomeCommand
{
    public string? CustomerRefId { get; set; }
    public string? IncomeSource { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }

}

public sealed class CreateCustomerIncomeCommandHandler : IRequestHandler<CreateCustomerIncomeCommand, Result<VCustomerIncomes>>
{
    private readonly ICustomerIncomeExpendService _cIncomeService;

    public CreateCustomerIncomeCommandHandler(ICustomerIncomeExpendService customerIncomeService)
    {
        _cIncomeService = customerIncomeService;
    }


    public async Task<Result<VCustomerIncomes>> Handle(CreateCustomerIncomeCommand request, CancellationToken cancellationToken)
    {
        var data = request.ToCreateCommand();

        var rsp = await _cIncomeService.AddOrUpdateIncomeAsync(request.CustomerRefId, data);

        return rsp;

    }
}


public sealed class CreateCustomerIncomesCommandHandler : IRequestHandler<List<CreateCustomerIncomeCommand>, Result<string>>
{
    private readonly ICustomerIncomeExpendService _cIncomeService;

    public CreateCustomerIncomesCommandHandler(ICustomerIncomeExpendService customerIncomeService)
    {
        _cIncomeService = customerIncomeService;
    }


    public async Task<Result<string>> Handle(List<CreateCustomerIncomeCommand> requests, CancellationToken cancellationToken)
    {

        var data = requests.Select(request => request.ToCreateCommand()).ToList();
       
        var customerId = requests.FirstOrDefault()?.CustomerRefId ?? "";

        if (customerId is null)
        {
            throw new NotFoundExceptions("CustomerRefId is required");
        }

        var rsp = await _cIncomeService.AddOrUpdateIncomesAsync(customerId, data);

        return rsp;

    }
}
