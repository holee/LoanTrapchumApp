using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;
using Applications.Commons.Mappers.Incomes;
using Applications.Features.Incomes.Command.CreateIncomeExpenseSource;

namespace Applications.Features.Incomes.Command.EditIncomeExpenseSource;

public sealed class EditIncomeExpenseCommandHandler : IRequestHandler<EditIncomeExpenseCommand, ErrorOr<int>>
{


    private readonly ICustomerIncomeExpendService _expendService;

    public EditIncomeExpenseCommandHandler(ICustomerIncomeExpendService expendService)
    {
        _expendService = expendService;
    }

    public Task<ErrorOr<int>> Handle(EditIncomeExpenseCommand request, CancellationToken cancellationToken = default)
    {
        var model = request.Request.ToIncomeExpenseEdit();

        return _expendService.AddOrUpdateIncomeExpenseAsync(model,cancellationToken);
    }
}