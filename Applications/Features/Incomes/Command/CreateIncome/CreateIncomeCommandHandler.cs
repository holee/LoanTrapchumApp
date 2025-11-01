using Applications.Commons.Mappers.Incomes;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Command.CreateIncome;

public sealed class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, ErrorOr<long>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public CreateIncomeCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<long>> Handle(CreateIncomeCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToDto();

        return await _custInomeSerivce.AddOrUpdateIncomeAsync(model);

    }
}
