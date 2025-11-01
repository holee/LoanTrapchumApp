using Applications.Commons.Messages.Incomes;
using Applications.Features.Incomes.Command.EditIncomes;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Command.EditIncome;

public sealed class EditIncomeCommandHandler : IRequestHandler<EditIncomeCommand, ErrorOr<long>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;
    public EditIncomeCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }
    public async Task<ErrorOr<long>> Handle(EditIncomeCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToDto();

        return await _custInomeSerivce.AddOrUpdateIncomeAsync(model);

    }
}
