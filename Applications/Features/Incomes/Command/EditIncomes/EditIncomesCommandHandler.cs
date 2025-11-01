using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Command.EditIncomes;
 
public sealed class EditIncomesCommandHandler : IRequestHandler<EditIncomesCommand, ErrorOr<string>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;
    public EditIncomesCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }
    public async Task<ErrorOr<string>> Handle(EditIncomesCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.ToDtos();

        return await _custInomeSerivce.AddOrUpdateIncomesAsync(request.CustomerId, model);

    }
}