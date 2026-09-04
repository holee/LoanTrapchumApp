using Application.Commons.Mappers.Incomes;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Command.CreateIncome;

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
