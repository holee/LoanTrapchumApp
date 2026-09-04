using Application.Commons.Messages.Incomes;
using Application.Features.Incomes.Command.EditIncomes;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Command.EditIncome;

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
