using Applications.Commons.Messages.Incomes;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.EditIncomeCommands;

public sealed class UpdateIncomeCommandHandler : IRequestHandler<UpdateIncomeCommand, ErrorOr<long>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;
    public UpdateIncomeCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }
    public async Task<ErrorOr<long>> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToDto();

        return await _custInomeSerivce.AddOrUpdateIncomeAsync(model);

    }
}


public sealed class UpdateIncomesCommandHandler : IRequestHandler<UpdateIncomesCommand, ErrorOr<string>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;
    public UpdateIncomesCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }
    public async Task<ErrorOr<string>> Handle(UpdateIncomesCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToDtos();

        return await _custInomeSerivce.AddOrUpdateIncomesAsync(request.CustomerId, model);

    }
}